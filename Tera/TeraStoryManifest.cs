﻿using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Tera.StoryStuff;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Tera
{
    internal class TeraStoryManifest : IStoryManifest
    {
        public static ExternalPartType? DemoPartType { get; private set; }
        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[0];
        public DirectoryInfo? GameRootFolder { get; set; }
        public ILogger Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name => "Teratto.Teramod.StoryManifest";

        private const string TERA_DECK_ID = "Teratto.TeraMod.Tera";

        public TeraStoryManifest()
        {
            Logger = null!;
        }

        private class NodeTypeConverter : JsonConverter<NodeType>
        {
            public override NodeType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? val = reader.GetString();
                return val switch
                {
                    "combat" => NodeType.combat,
                    "event" => NodeType.@event,
                    "voidShout" => NodeType.voidShout,
                    _ => throw new InvalidOperationException(),
                };
            }

            public override void Write(Utf8JsonWriter writer, NodeType value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class StatusConverter : JsonConverter<Status>
        {
            private readonly ILogger _logger;

            private Dictionary<string, Status> _teraStatuses;

            public StatusConverter(ILogger logger)
            {
                _logger = logger;

                _teraStatuses = typeof(TeraModStatuses).GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Where(f => f.FieldType == typeof(Status))
                    .ToDictionary(f => f.Name, f => (Status)(f.GetValue(null) ?? 0));
            }

            public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? val = reader.GetString();

                if (_teraStatuses.TryGetValue(val ?? "", out Status status))
                {
                    return status;
                }

                if (!Enum.TryParse<Status>(val, out Status result))
                {
                    _logger.LogError("Cannot parse \"{Val}\" to enum {EnumName}", val, typeof(Deck).FullName);
                }
                return result;
            }

            public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class DeckConverter : JsonConverter<Deck>
        {
            private readonly ILogger _logger;

            public DeckConverter(ILogger logger)
            {
                _logger = logger;
            }

            public override Deck Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? val = reader.GetString();

                if (val == "tera" && ModManifest.tera_deck!.Id.HasValue)
                {
                    return (Deck)ModManifest.tera_deck!.Id.Value;
                }

                if (!Enum.TryParse<Deck>(val, out Deck result))
                {
                    _logger.LogError("Cannot parse \"{Val}\" to enum {EnumName}", val, typeof(Deck).FullName);
                }
                return result;
            }

            public override void Write(Utf8JsonWriter writer, Deck value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }


        private class GenericEnumConverter<T> : JsonConverter<T> where T : struct
        {
            private readonly ILogger _logger;

            public GenericEnumConverter(ILogger logger)
            {
                _logger = logger;
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? val = reader.GetString();
                if (!Enum.TryParse<T>(val, out T result))
                {
                    _logger.LogError("Cannot parse \"{Val}\" to enum {EnumName}", val, typeof(T).FullName);
                }
                return result;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        public class InjectedItem
        {
            [JsonPropertyName("event_name")]
            public string EventName;

            [JsonPropertyName("indices")]
            public int[] Indices = Array.Empty<int>();

            [JsonPropertyName("who")]
            public string Who;

            [JsonPropertyName("what")]
            public string What;

            [JsonPropertyName("loop_tag")]
            public string LoopTag;
        }

        public class InjectedItemContainer
        {
            [JsonPropertyName("items")]
            public InjectedItem[] Items;
        }

        public class InstructionListWrapper
        {
            private List<Instruction>? _instructions = new();
            private List<Say>? _says;

            public void Set(List<Instruction> instructions)
            {
                _instructions = instructions;
                _says = null;
            }

            public void Set(List<Say> says)
            {
                _instructions = null;
                _says = says;
            }

            public void Add(SaySwitch saySwitch)
            {
                if (_instructions == null)
                {
                    // TODO: Log instead!
                    throw new InvalidOperationException();
                }
                _instructions.Add(saySwitch);
            }

            public void Add(Say say)
            {
                if (_instructions != null)
                {
                    _instructions.Add(say);
                }
                else
                {
                    _says!.Add(say);
                }
            }

            public Instruction this[int i]
            {
                get
                {
                    if (_instructions != null)
                    {
                        return _instructions[i];
                    }
                    else
                    {
                        return _says![i];
                    }
                }
            }

            public int Count
            {
                get
                {
                    if (_instructions != null)
                    {
                        return _instructions.Count;
                    }
                    return _says!.Count;
                }
            }
        }

        private static TeraStoryManifest _instance = null!;
        private InjectedItemContainer _container = null!;
        private Dictionary<string, StoryNode> _nodesToInject = null!;

        private static Queue<ValueTuple<string, Action>> MakeInitQueue_Postfix(Queue<ValueTuple<string, Action>> __result)
        {
            // we apply any patches to story item.
            __result.Enqueue(new("patching story for tera", () => { _instance.PatchStory(); }));
            return __result;
        }

        public void PatchStory()
        {
            foreach (InjectedItem item in _container.Items)
            {
                if (!DB.story.all.TryGetValue(item.EventName, out StoryNode? node))
                {
                    if (!_nodesToInject.TryGetValue(item.EventName, out node))
                    {
                        Console.WriteLine($"TeraMod: Cannot find StoryNode named {item.EventName} in either stock DB or mod StoryNodes. Is one of the files out of date?");
                        continue;
                    }

                    DB.story.all[item.EventName] = node;
                }

                InstructionListWrapper instructions = new();
                instructions.Set(node.lines);
                Instruction? targetInstruction = null;

                for (int level = 0; level < item.Indices.Length; ++level)
                {
                    int index = item.Indices[level];

                    while (index >= instructions.Count)
                    {
                        if (level + 1 < item.Indices.Length)
                        {
                            instructions.Add(new SaySwitch()
                            {
                                lines = new List<Say>()
                            }); ;
                        }
                        else
                        {
                            instructions.Add(new Say());
                        }
                    }

                    targetInstruction = instructions[index];

                    if (targetInstruction is SaySwitch saySwitch)
                    {
                        instructions.Set(saySwitch.lines);
                    }
                    else
                    {
                        break;
                    }
                }

                if (targetInstruction is not Say say)
                {
                    Console.WriteLine($"Failed to inject a line into story {item.EventName} at index {string.Join(',',item.Indices)} with what = '{item.What}'!!");
                    continue;
                }

                say.who = item.Who.ToLower() == "tera" ? "Teratto.TeraMod.Tera" : item.Who;
                say.hash = GetHash(item.What);
                say.loopTag = string.IsNullOrEmpty(item.LoopTag) ? null : item.LoopTag;
            }
        }


        private static string GetHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var inputHash = SHA256.HashData(inputBytes);
            return Convert.ToHexString(inputHash); //.Substring(0, 8);
        }

        private static void LoadStringsForLocale_PostFix(string locale, ref Dictionary<string, string> __result)
        {
            if (locale != "en")
            {
                // Sorry, not localized ;-;
                return;
            }

            foreach (InjectedItem item in _instance._container.Items)
            {
                string hash = GetHash(item.What);
                string key = $"{item.EventName}:{hash}";
                __result[key] = item.What;
            }
        }

        public void LoadManifest(IStoryRegistry storyRegistry)
        {
            _instance = this;

            using (FileStream stream = File.OpenRead(Path.Join(_instance.ModRootFolder!.FullName, "story.json")))
            {
                _container = JsonSerializer.Deserialize<InjectedItemContainer>(stream, new JsonSerializerOptions()
                {
                    IncludeFields = true
                })!;
            }

            using (FileStream stream = File.OpenRead(Path.Join(_instance.ModRootFolder!.FullName, "story_nodes.json")))
            {
                JsonSerializerOptions options = new() { IncludeFields = true };
                options.Converters.Add(new NodeTypeConverter());
                options.Converters.Add(new StatusConverter(Logger));
                options.Converters.Add(new DeckConverter(Logger));

                _nodesToInject = JsonSerializer.Deserialize<Dictionary<string, StoryNode>>(stream, options)!;
            }
                
            Harmony harmony = new Harmony(Name);

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(DB), nameof(DB.MakeInitQueue)),
                postfix: new HarmonyMethod(AccessTools.DeclaredMethod(GetType(), nameof(MakeInitQueue_Postfix)))
            );
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(DB), nameof(DB.LoadStringsForLocale)),
                postfix: new HarmonyMethod(AccessTools.DeclaredMethod(GetType(), nameof(LoadStringsForLocale_PostFix)))
            );
        }

        public void LoadManifestExample(IStoryRegistry storyRegistry)
        {
            // A combat shout
            var exampleShout = new ExternalStory("EWanderer.Demomod.DemoStory.CombatShout",
                new StoryNode() // Native CobaltCore class, containing numerous options regarding the shout's trigger. Listed are only the most common, but feel free to explore
                {
                    type = NodeType.combat, // Mark the story as a combat shout
                    priority = true, // Forces this story to be selected before other valid ones when the database is queried, useful for debugging.

                    once = false,
                    oncePerCombat = false,
                    oncePerRun = false, // Self explanatory

                    lookup = new HashSet<string>() // This is a list of tags that queries look for in various situations, very useful for triggering shouts in specific situations !
                    {
                        "demoCardShout" // We'll feed this string to a CardAction's dialogueSelector field in EWandererDemoCard, so that this shout triggers when we play the upgrade B of the card
                    },
                    
                    allPresent = new HashSet<string>() // this checks for the presence of a list of characters.
                    {
                        "riggs"
                    }
                },
                new List<object>() /* this is the actual dialogue. You can feed this list :
                                    * classes inheriting from Instruction (natively Command, Say, or Sayswitch)
                                    * ExternalStory.ExternalSay, which act as a native Say, but automating the more tedious parts,
                                    * such as localizing and hashing*/
                {
                    new ExternalStory.ExternalSay()
                    {
                        Who = "riggs", /* the character that talks. For modded characters, use CharacterDeck.GlobalName
                                        * attempting to make an absent character speak in combat will interrupt the shout !*/
                        What = "Example shout !",
                        LoopTag = "squint" // the specific animation that should play during the shout. "neutral" is default
                    },
                    //new Say() // same as above, but native
                   // {
                    //    who = "peri",
                   //     hash = "0" // a string that must be unique to your story, used to fetch localisation 
                  //  },
                    new ExternalStory.ExternalSaySwitch( new List<ExternalStory.ExternalSay>() // this is used to randomly pick a valid options among the listed Says.
                    {
                        new ExternalStory.ExternalSay()
                        {
                            Who = "dizzy",
                            What = "A !",
                            LoopTag = "squint" 
                        },
                        new ExternalStory.ExternalSay()
                        {
                            Who = "eunice",
                            What = "B !",
                            LoopTag = "squint"
                        },
                        new ExternalStory.ExternalSay()
                        {
                            Who = "goat",
                            What = "C !",
                            LoopTag = "squint"
                        },
                    }) 
                                     
                }
            );
           // exampleShout.AddLocalisation("0", "Example native shout !"); // setting the localisation for peri's shout using the native way

            storyRegistry.RegisterStory(exampleShout);

            var exampleEvent = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent",
                    node: new StoryNode()
                    {
                        type = NodeType.@event,// Mark the story as an event
                        priority = true,
                        canSpawnOnMap = true, // self explanatory, dictate whether the event can be a [?] node on the map

                        zones = new HashSet<string>() // dictate in which zone of the game the event can trigger.
                        {
                            "zone_first"
                            //"zone_lawless"
                            //"zone_three"
                            //"zone_magic"
                            //"zone_finale"
                        },

                        /*lookup = new HashSet<string>() 
                        {
                            Lookup for event have some interesting functionalities. For exemple, the tag before_[EnemyName] or after_[EnemyName] will
                            make it so the event triggers right before or after said enemy combat, as done with the mouse knight guy for example
                        },*/

                        choiceFunc = "demoChoiceFunc", /* This triggers a registered choice function at the end of the dialogue.
                                                        * You can see vanilla examples in the class Events*/
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            Who = "walrus", // characters in event dialogues don't need to be actually present !
                            What = "Example event start !",
                            Flipped = true, // if true, the character is on the right side of the dialogue while talking
                        },
                        new Command(){name = "demoDoStuffCommand"}, /* execute a registered method, only works during dialogues.
                                                                     * You can see vanilla examples in the class StoryCommands*/
                        new ExternalStory.ExternalSay()
                        {
                            Who = "comp",
                            What = "Ouch !",
                        },
                    }
                );

            storyRegistry.RegisterChoice("demoChoiceFunc", typeof(DemoStoryChoices).GetMethod(nameof(DemoStoryChoices.DemoStoryChoice))!);
            storyRegistry.RegisterCommand("demoDoStuffCommand", typeof(DemoStoryCommands).GetMethod(nameof(DemoStoryCommands.DemoStoryCommand))!);
            storyRegistry.RegisterStory(exampleEvent);

            var exampleEventOutcome_0 = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent_Outcome_0",
                    node: new StoryNode()
                    {
                        type = NodeType.@event,
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            Who = "comp",
                            What = "Yay !",
                        },
                    }
                );
            storyRegistry.RegisterStory(exampleEventOutcome_0);

            var exampleEventOutcome_1 = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent_Outcome_1",
                    node: new StoryNode()
                    {
                        type = NodeType.@event,
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            Who = "comp",
                            What = "That hurts !",
                        },
                    }
                );
            storyRegistry.RegisterStory(exampleEventOutcome_1);

            var exampleEventOutcome_2 = new ExternalStory("EWanderer.Demomod.DemoStory.ChoiceEvent_Outcome_2",
                    node: new StoryNode()
                    {
                        type = NodeType.@event
                    },
                    instructions: new List<object>()
                    {
                        new ExternalStory.ExternalSay()
                        {
                            Who = "comp",
                            What = "Let's scoot !",
                        },
                    }
                );
            storyRegistry.RegisterStory(exampleEventOutcome_2);

            // Story injectors below, allows you to actively edit/insert stuff from existing story in a non destructive way !

            var injector = new ExternalStoryInjector("AbandonedShipyard", // the story in which to insert stuff
                                                    ExternalStoryInjector.QuickInjection.Beginning, // from where to insert stuff. SaySwitch will insert your stuff in the nth SaySwitch, where n is targetIndex 
                                                    1, // an index that offset the quick injection location. in this case, we're inserting 1 away from the beginning.
                                                    new List<object>() // the stuff you want to insert
            {
                new ExternalStory.ExternalSay()
                {
                    Who = "comp",
                    What = "Hey, i'm not supposed to say that !"
                }
            });

            storyRegistry.RegisterInjector(injector);


            // Advanced injectors let you hook a method that will receive the StoryNode during DB injection time, allowing you to modify it however you wish !

            var advancedInjector = new ExternalStoryInjector("AbandonedShipyard", (node) =>
            {
                StoryNode s = node as StoryNode;


            });
            storyRegistry.RegisterInjector(advancedInjector);
        }
    }
}