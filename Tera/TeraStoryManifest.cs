using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Tera.StoryInjector;

namespace Tera
{
    internal class TeraStoryManifest : IStoryManifest
    {
        public IEnumerable<DependencyEntry> Dependencies => Array.Empty<DependencyEntry>();
        public DirectoryInfo? GameRootFolder { get; set; }
        public ILogger Logger { get; set; } = null!;
        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name => "Teratto.Teramod.StoryManifest";

        private const string TERA_DECK_ID = "Teratto.TeraMod.Tera";

        private static TeraStoryManifest _instance = null!;
        private InjectedItemContainer _container = null!;
        private Dictionary<string, StoryNode> _nodesToInject = null!;

        private static Queue<ValueTuple<string, Action>> MakeInitQueue_Postfix(Queue<ValueTuple<string, Action>> __result)
        {
            // we apply any patches to story item.
            __result.Enqueue(new("patching story for tera", () => { _instance.PatchStory(); }));
            return __result;
        }
        
        private void PatchStory()
        {
            foreach (InjectedItem item in _container.Items)
            {
                if (!DB.story.all.TryGetValue(item.NodeName, out StoryNode? node))
                {
                    if (!_nodesToInject.TryGetValue(item.NodeName, out node))
                    {
                        Logger?.LogWarning("TeraMod: Cannot find StoryNode named {NodeName} in either stock DB or mod StoryNodes. Is one of the files out of date?", item.NodeName);
                        continue;
                    }

                    DB.story.all[item.NodeName] = node;
                }

                InstructionListWrapper instructions = new();
                instructions.Set(node.lines);
                Instruction? targetInstruction = null;

                for (int level = 0; level < item.Indices.Length; ++level)
                {
                    int index = item.Indices[level];

                    if (item.IsInserted && level + 1 == item.Indices.Length) 
                    {
                        index = instructions.Count;
                    }

                    while (index >= instructions.Count)
                    {
                        if (level + 1 < item.Indices.Length)
                        {
                            instructions.Add(new SaySwitch()
                            {
                                lines = new List<Say>()
                            });
                        }
                        else
                        {
                            instructions.Add(new Say() {
                                who = TERA_DECK_ID,
                                hash = "Missing line... I should double check my indices!"
                            });
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
                    Console.WriteLine($"Failed to inject a line into story {item.NodeName} at index {string.Join(',',item.Indices)} with what = '{item.What}'!!");
                    continue;
                }

                say.who = item.Who.ToLower() == "tera" ? TERA_DECK_ID : item.Who;
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
                string key = $"{item.NodeName}:{hash}";
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
        
    }
}
