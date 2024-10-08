﻿using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Tera.Cards;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using Shockah.Kokoro;
using Tera.StatusPatches;
using System.IO;
using System;

namespace Tera
{
    public class ModManifest : IModManifest, IPrelaunchManifest, ISpriteManifest, IAnimationManifest, IDeckManifest, ICardManifest, ICardOverwriteManifest, ICharacterManifest, IGlossaryManifest, IArtifactManifest, IStatusManifest, ICustomEventManifest
    {
        public static ExternalStatus? demo_status;
        internal static int x = 0;
        public static ExternalDeck? TeraDeck;

        private ISpriteRegistry? sprite_registry;
        private IAnimationRegistry? animation_registry;
        private IDeckRegistry? deck_registry;

        private Harmony harmony;

        public IEnumerable<DependencyEntry> Dependencies { get; }
        public DirectoryInfo? GameRootFolder { get; set; }
        public ILogger? Logger { get; set; } = null!;
        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name => "Teratto.TeraMod.MainManifest";

        public static IKokoroApi Kokoro = null!;
        
        private static ModManifest _instance = null!; 
        
        // Constructor!! :D
        public ModManifest()
        {
            Dependencies = new DependencyEntry[] {
            };
        }

        public void BootMod(IModLoaderContact contact)
        {
            _instance = this;

            harmony = new Harmony(Name);

            TaxationStatusPatches.Apply(harmony, Logger);
            StallAndLockNextTurnStatusPatches.Apply(harmony, Logger);
            InterestStatusPatches.Apply(harmony, Logger);
            AStatusBeginPatches.Apply(harmony, Logger);
            TeraModCardInterfacePatch.Apply(harmony, Logger);

            Colors.colorDict["tera"] = 0xff266fd8;

            Kokoro = contact.GetApi<IKokoroApi>("Shockah.Kokoro") ?? throw new InvalidOperationException("Failed to load Kokoro API!");
        }

        /// <summary>
        /// Convenience method to load a sprite! Call this from the LoadManifest method below!
        /// </summary>
        private ExternalSprite LoadSprite(ISpriteRegistry artRegistry, string globalName, string spritesPath)
        {
            var path = Path.Combine(ModRootFolder!.FullName, "Sprites", spritesPath);
            ExternalSprite sprite = new ExternalSprite(globalName, new FileInfo(path));
            if (!artRegistry.RegisterArt(sprite))
                throw new Exception("Cannot register sprite.");
            return sprite;
        }

        /// <summary>
        /// Load and register your sprites here!
        /// </summary>
        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            if (ModRootFolder == null)
                throw new Exception("No root folder set!");

            sprite_registry = artRegistry;

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Blush1", "BlushIdle\\Bird_Blush_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Blush2", "BlushIdle\\Bird_Blush_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Blush3", "BlushIdle\\Bird_Blush_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Blush4", "BlushIdle\\Bird_Blush_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Blush5", "BlushIdle\\Bird_Blush_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Closed1", "ClosedIdle\\Bird_eyesClosed_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Closed2", "ClosedIdle\\Bird_eyesClosed_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Closed3", "ClosedIdle\\Bird_eyesClosed_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Closed4", "ClosedIdle\\Bird_eyesClosed_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Closed5", "ClosedIdle\\Bird_eyesClosed_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Happy1", "HappyIdle\\Bird_happy_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Happy2", "HappyIdle\\Bird_happy_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Happy3", "HappyIdle\\Bird_happy_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Happy4", "HappyIdle\\Bird_happy_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Happy5", "HappyIdle\\Bird_happy_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.LookAway1", "LookAwayIdle\\Bird_lookAway_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.LookAway2", "LookAwayIdle\\Bird_lookAway_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.LookAway3", "LookAwayIdle\\Bird_lookAway_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.LookAway4", "LookAwayIdle\\Bird_lookAway_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.LookAway5", "LookAwayIdle\\Bird_lookAway_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Nervous1", "LookAwayIdleNervousIdle\\Bird_lookAwayNervous_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Nervous2", "LookAwayIdleNervousIdle\\Bird_lookAwayNervous_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Nervous3", "LookAwayIdleNervousIdle\\Bird_lookAwayNervous_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Nervous4", "LookAwayIdleNervousIdle\\Bird_lookAwayNervous_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Nervous5", "LookAwayIdleNervousIdle\\Bird_lookAwayNervous_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Neutral1", "NormalIdle\\Bird_neutral_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Neutral2", "NormalIdle\\Bird_neutral_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Neutral3", "NormalIdle\\Bird_neutral_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Neutral4", "NormalIdle\\Bird_neutral_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Neutral5", "NormalIdle\\Bird_neutral_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Sad1", "SadIdle\\Bird_Sad_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Sad2", "SadIdle\\Bird_Sad_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Sad3", "SadIdle\\Bird_Sad_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Sad4", "SadIdle\\Bird_Sad_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Sad5", "SadIdle\\Bird_Sad_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Scared1", "ScaredIdle\\Bird_scared_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Scared2", "ScaredIdle\\Bird_scared_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Scared3", "ScaredIdle\\Bird_scared_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Scared4", "ScaredIdle\\Bird_scared_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Scared5", "ScaredIdle\\Bird_scared_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Squint1", "SquintIdle\\Bird_squint_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Squint2", "SquintIdle\\Bird_squint_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Squint3", "SquintIdle\\Bird_squint_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Squint4", "SquintIdle\\Bird_squint_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Squint5", "SquintIdle\\Bird_squint_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.HappyTaxBoi1", "TaxesHappyIdle\\Bird_taxesHappy_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.HappyTaxBoi2", "TaxesHappyIdle\\Bird_taxesHappy_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.HappyTaxBoi3", "TaxesHappyIdle\\Bird_taxesHappy_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.HappyTaxBoi4", "TaxesHappyIdle\\Bird_taxesHappy_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.HappyTaxBoi5", "TaxesHappyIdle\\Bird_taxesHappy_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.TaxBoi1", "TaxesIdle\\Bird_taxes_1.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.TaxBoi2", "TaxesIdle\\Bird_taxes_2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.TaxBoi3", "TaxesIdle\\Bird_taxes_3.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.TaxBoi4", "TaxesIdle\\Bird_taxes_4.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.TaxBoi5", "TaxesIdle\\Bird_taxes_5.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.TeraBorder", "border_tera.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Mini1", "bird_mini_0.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.Panel", "panel_tera.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.Tera.GameOver", "bird_GameOver_0.png");

            LoadSprite(artRegistry, "Teratto.Teramod.Tera.coin", "coin.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.coin", "coin.png");

            LoadSprite(artRegistry, "Teratto.Teramod.MissingTera", "missingTera.png");

            LoadSprite(artRegistry, "Teratto.Teramod.Taxes", "taxes.png");
            LoadSprite(artRegistry, "Teratto.Teramod.StallNext", "Stall Next.png");
            LoadSprite(artRegistry, "Teratto.Teramod.LockNext", "LockNext.png");
            LoadSprite(artRegistry, "Teratto.Teramod.BailoutBeta", "BailoutBeta.png");
            LoadSprite(artRegistry, "Teratto.Teramod.Grant", "Grant.png");
            LoadSprite(artRegistry, "Teratto.Teramod.FlightTraining", "FlightTraining.png");
            LoadSprite(artRegistry, "Teratto.Teramod.YearlyPayments", "YearlyPayments.png");
            LoadSprite(artRegistry, "Teratto.Teramod.Capitalism", "Capitalism.png");
            LoadSprite(artRegistry, "Teratto.Teramod.EarlyBird", "EarlyBird.png");
            LoadSprite(artRegistry, "Teratto.Teramod.TeraEgg", "teraegg.png");
            LoadSprite(artRegistry, "Teratto.Teramod.TeraInflation", "ArtifactInflation.png");

            LoadSprite(artRegistry, "Teratto.TeraMod.CardEgg", "CardBgs\\CardEgg.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardBailout", "CardBgs\\CardBailout.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardBreakout", "CardBgs\\CardBreakout.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardPayment", "CardBgs\\CardPayment.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardSalesTax", "CardBgs\\CardSalesTax.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardHeftyTax", "CardBgs\\CardHeftyTax.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardNumberCrunch", "CardBgs\\CardNumberCrunch.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardAudit", "CardBgs\\CardAudit.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardEggShells", "CardBgs\\cardEggShells.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardAllIn", "CardBgs\\cardAllIn.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardDesperation", "CardBgs\\cardDesperatioon.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardForgiveness", "CardBgs\\cardForgiveness.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardGetsTheWorm", "CardBgs\\cardGetsTheWorm.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardTariff", "CardBgs\\cardTariff.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardTaxFraud", "CardBgs\\cardTaxFraud.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardTaxingEscape", "CardBgs\\cardTaxingEscape.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardTenacity", "CardBgs\\cardTenacity.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardMarketCrash", "CardBgs\\cardMarketCrash.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardTaxExemption", "CardBgs\\cardTaxExemption.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardHealthInsurance", "CardBgs\\cardHealthInsurance.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardTaunt", "CardBgs\\cardTaunt.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardTaunt2", "CardBgs\\cardTaunt2.png");
            LoadSprite(artRegistry, "Teratto.TeraMod.CardGetaway", "CardBgs\\cardGetaway.png");
        }

        /// <summary>
        /// Load animations in this Method.
        /// </summary>
        public void LoadManifest(IAnimationRegistry registry)
        {
            animation_registry = registry;

            //VVVV Animation registry
            ExternalAnimation blushAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.blush", TeraDeck, "blush", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Blush1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Blush2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Blush3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Blush4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Blush5")
            });
            registry.RegisterAnimation(blushAnimation);

            ExternalAnimation closedAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.closed", TeraDeck, "closed", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Closed1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Closed2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Closed3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Closed4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Closed5")
            });
            registry.RegisterAnimation(closedAnimation);

            ExternalAnimation happyAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.happy", TeraDeck, "happy", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Happy1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Happy2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Happy3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Happy4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Happy5")
            });
            registry.RegisterAnimation(happyAnimation);

            ExternalAnimation lookawayAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.lookaway", TeraDeck, "lookaway", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.LookAway1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.LookAway2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.LookAway3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.LookAway4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.LookAway5")
            });
            registry.RegisterAnimation(lookawayAnimation);

            ExternalAnimation nervousAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.nervous", TeraDeck, "nervous", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Nervous1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Nervous2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Nervous3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Nervous4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Nervous5")
            });
            registry.RegisterAnimation(nervousAnimation);

            ExternalAnimation neutralAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.neutral", TeraDeck, "neutral", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Neutral1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Neutral2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Neutral3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Neutral4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Neutral5")
            });
            registry.RegisterAnimation(neutralAnimation);

            ExternalAnimation sadAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.sad", TeraDeck, "sad", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Sad1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Sad2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Sad3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Sad4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Sad5")
            });
            registry.RegisterAnimation(sadAnimation);

            ExternalAnimation scaredAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.scared", TeraDeck, "scared", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Scared1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Scared2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Scared3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Scared4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Scared5")
            });
            registry.RegisterAnimation(scaredAnimation);

            ExternalAnimation squintAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.squint", TeraDeck, "squint", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Squint1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Squint2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Squint3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Squint4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Squint5")
            });
            registry.RegisterAnimation(squintAnimation);

            ExternalAnimation happytaxAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.happytax", TeraDeck, "happytax", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.HappyTaxBoi1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.HappyTaxBoi2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.HappyTaxBoi3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.HappyTaxBoi4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.HappyTaxBoi5")
            });
            registry.RegisterAnimation(happytaxAnimation);

            ExternalAnimation taxneutralAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.taxneutral", TeraDeck, "taxneutral", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.TaxBoi1"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.TaxBoi2"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.TaxBoi3"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.TaxBoi4"),
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.TaxBoi5")
            });
            registry.RegisterAnimation(taxneutralAnimation);

            ExternalAnimation miniAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.mini", TeraDeck, "mini", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Mini1")
            });
            registry.RegisterAnimation(miniAnimation);

            ExternalAnimation coinAnimation = new ExternalAnimation("Teratto.Teramod.Tera.coin", TeraDeck, "coin", false, new ExternalSprite[] {
                sprite_registry!.LookupSprite("Teratto.Teramod.Tera.coin")
            });
            registry.RegisterAnimation(coinAnimation);
            ExternalAnimation deathAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.GameOver", TeraDeck, "gameover", false, new ExternalSprite[]
            {
                sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.GameOver"),
            });
            registry.RegisterAnimation(deathAnimation);

            ExternalAnimation eggAnimation = new ExternalAnimation("Teratto.TeraMod.Tera.TeraEgg", TeraDeck, "eggie", false, new ExternalSprite[]
            {
                sprite_registry!.LookupSprite("Teratto.Teramod.TeraEgg"),
            });
            registry.RegisterAnimation(eggAnimation);
        }

        /// <summary>
        /// Set up and register custom decks here!
        /// </summary>
        public void LoadManifest(IDeckRegistry registry)
        {
            deck_registry = registry;

            ExternalSprite art = ExternalSprite.GetRaw((int)Spr.cards_colorless);
            ExternalSprite border = sprite_registry.LookupSprite("Teratto.TeraMod.TeraBorder");

            TeraDeck = new ExternalDeck("Teratto.TeraMod.Tera", System.Drawing.Color.FromArgb(0x26, 0x6f, 0xd8), System.Drawing.Color.Black, art, border, null);
            
            if (!registry.RegisterDeck(TeraDeck)) {
                Logger?.LogError("Failed to register Tera deck!");
            }
        }

        /// <summary>
        /// Set up custom cards here!
        /// </summary>
        /// <param name="registry"></param>
        public void LoadManifest(ICardRegistry registry)
        {
            ExternalCard teraEggCard = new ExternalCard("Teratto.TeraMod.TeraEgg", typeof(TeraCardEgg), sprite_registry.LookupSprite("Teratto.TeraMod.CardEgg"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            //add card name in english
            teraEggCard.AddLocalisation("Egg Toss");
            //register card in the db extender.
            registry.RegisterCard(teraEggCard);

            ExternalCard teraCardCower = new ExternalCard("Teratto.TeraMod.TeraCower", typeof(TeraCardCower), sprite_registry.LookupSprite("Teratto.TeraMod.CardTaunt2"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardCower.AddLocalisation("Taunt");
            registry.RegisterCard(teraCardCower);

            ExternalCard teraCardCaw = new ExternalCard("Teratto.TeraMod.TeraCaw", typeof(TeraCardCaw), sprite_registry.LookupSprite("Teratto.TeraMod.CardBreakout"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardCaw.AddLocalisation("Breakout");
            registry.RegisterCard(teraCardCaw);

            ExternalCard teraCardGetaway = new ExternalCard("Teratto.TeraMod.TeraGetaway", typeof(TeraCardGetaway), sprite_registry.LookupSprite("Teratto.TeraMod.CardGetaway"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardGetaway.AddLocalisation("Frenzied Getaway");
            registry.RegisterCard(teraCardGetaway);

            ExternalCard teraCardPanic = new ExternalCard("Teratto.TeraMod.TeraPanic", typeof(TeraCardPanic), sprite_registry.LookupSprite("Teratto.TeraMod.CardNumberCrunch"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardPanic.AddLocalisation("Number Crunching");
            registry.RegisterCard(teraCardPanic);

            ExternalCard teraCardTariff = new ExternalCard("Teratto.TeraMod.TeraTariff", typeof(TeraCardTariff), sprite_registry.LookupSprite("Teratto.TeraMod.CardTariff"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardTariff.AddLocalisation("Tariff");
            registry.RegisterCard(teraCardTariff);

            ExternalCard teraCardAudit = new ExternalCard("Teratto.TeraMod.TeraAudit", typeof(TeraCardAudit), sprite_registry.LookupSprite("Teratto.TeraMod.CardAudit"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardAudit.AddLocalisation("Audit");
            registry.RegisterCard(teraCardAudit);

            ExternalCard teraCardMarketCrash = new ExternalCard("Teratto.TeraMod.TeraMarketCrash", typeof(TeraMarketCrash), sprite_registry.LookupSprite("Teratto.TeraMod.CardMarketCrash"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardMarketCrash.AddLocalisation("Market Crash");
            registry.RegisterCard(teraCardMarketCrash);

            ExternalCard teraCardRefund = new ExternalCard("Teratto.TeraMod.TeraRefund", typeof(TeraCardRefund), sprite_registry.LookupSprite("Teratto.TeraMod.CardTaxExemption"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardRefund.AddLocalisation("Tax Evasion");
            registry.RegisterCard(teraCardRefund);

            ExternalCard teraCardTaxRun = new ExternalCard("Teratto.TeraMod.TeraTaxRun", typeof(TeraCardTaxRun), sprite_registry.LookupSprite("Teratto.TeraMod.CardTaxingEscape"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardTaxRun.AddLocalisation("Taxing Escape");
            registry.RegisterCard(teraCardTaxRun);

            ExternalCard teraCardExemption = new ExternalCard("Teratto.TeraMod.TeraExemption", typeof(TeraCardExemption), sprite_registry.LookupSprite("Teratto.TeraMod.CardTaxExemption"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardExemption.AddLocalisation("Tax Exemption");
            registry.RegisterCard(teraCardExemption);

            ExternalCard teraCardDesperation = new ExternalCard("Teratto.TeraMod.TeraDesperation", typeof(TeraCardDesperation), sprite_registry.LookupSprite("Teratto.TeraMod.CardDesperation"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardDesperation.AddLocalisation("Desperation");
            registry.RegisterCard(teraCardDesperation);

            ExternalCard teraCardStrength = new ExternalCard("Teratto.TeraMod.TeraStrength", typeof(TeraCardStrength), sprite_registry.LookupSprite("Teratto.TeraMod.CardForgiveness"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"), new[] { ConditionalGlossaryKey });
            teraCardStrength.AddLocalisation("Forgiveness");
            registry.RegisterCard(teraCardStrength);

            ExternalCard teraCardSalesTax = new ExternalCard("Teratto.TeraMod.TeraSalesTax", typeof(TeraCardSalesTax), sprite_registry.LookupSprite("Teratto.TeraMod.CardSalesTax"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardSalesTax.AddLocalisation("Sales Tax");
            registry.RegisterCard(teraCardSalesTax);

            ExternalCard teraCardPersistence = new ExternalCard("Teratto.TeraMod.teraPersistence", typeof(TeraCardPersistence), sprite_registry.LookupSprite("Teratto.TeraMod.CardTaunt"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardPersistence.AddLocalisation("Persistence");
            registry.RegisterCard(teraCardPersistence);

            ExternalCard teraCardHefty = new ExternalCard("Teratto.TeraMod.teraHefty", typeof(TeraCardHefty), sprite_registry.LookupSprite("Teratto.TeraMod.CardHeftyTax"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardHefty.AddLocalisation("Hefty Tax");
            registry.RegisterCard(teraCardHefty);

            ExternalCard teraCardFraud = new ExternalCard("Teratto.TeraMod.teraFraud", typeof(TeraCardFraud), sprite_registry.LookupSprite("Teratto.TeraMod.CardTaxFraud"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardFraud.AddLocalisation("Tax Fraud");
            registry.RegisterCard(teraCardFraud);

            ExternalCard teraCardWorm = new ExternalCard("Teratto.TeraMod.teraWorm", typeof(TeraCardWorm), sprite_registry.LookupSprite("Teratto.TeraMod.CardGetsTheWorm"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardWorm.AddLocalisation("Gets the Worm");
            registry.RegisterCard(teraCardWorm);

            ExternalCard teraCardHealthInsurance = new ExternalCard("Teratto.TeraMod.teraHealthInsurance", typeof(TeraCardHealthInsurance), sprite_registry.LookupSprite("Teratto.TeraMod.CardHealthInsurance"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardHealthInsurance.AddLocalisation("Health Insurance");
            registry.RegisterCard(teraCardHealthInsurance);

            ExternalCard teraCardBailout = new ExternalCard("Teratto.TeraMod.teraBailout", typeof(TeraCardBailout), sprite_registry.LookupSprite("Teratto.TeraMod.CardBailout"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardBailout.AddLocalisation("Bailout");
            registry.RegisterCard(teraCardBailout);

            ExternalCard teraCardTenacity = new ExternalCard("Teratto.TeraMod.teraTenacity", typeof(TeraCardTenacity), sprite_registry.LookupSprite("Teratto.TeraMod.CardTenacity"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardTenacity.AddLocalisation("Tenacity");
            registry.RegisterCard(teraCardTenacity);

            ExternalCard teraCardAllIn = new ExternalCard("Teratto.TeraMod.teraAllIn", typeof(TeraCardAllIn), sprite_registry.LookupSprite("Teratto.TeraMod.CardAllIn"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardAllIn.AddLocalisation("All-In");
            registry.RegisterCard(teraCardAllIn);

            ExternalCard teraCardEggShells = new ExternalCard("Teratto.TeraMod.teraEggShells", typeof(TeraCardEggShells), sprite_registry.LookupSprite("Teratto.TeraMod.CardEggShells"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardEggShells.AddLocalisation("Egg Shells");
            registry.RegisterCard(teraCardEggShells);

            ExternalCard teraCardPayment = new ExternalCard("Teratto.TeraMod.teraPayment", typeof(TeraCardPayment), sprite_registry.LookupSprite("Teratto.TeraMod.CardPayment"), deck_registry!.LookupDeck("Teratto.TeraMod.Tera"));
            teraCardPayment.AddLocalisation("Payment");
            registry.RegisterCard(teraCardPayment);
        }

        /// <summary>
        /// Register any card overwrites here! Card overwrites change cards that exist in the base game.
        /// </summary>
        public void LoadManifest(ICardOverwriteRegistry registry)
        {
        }

        public void LoadManifest(ICharacterRegistry registry)
        {
            var tera_spr = sprite_registry!.LookupSprite("Teratto.TeraMod.Tera.Panel");

            ExternalAnimation neutralAnimation = animation_registry!.LookupAnimation("Teratto.TeraMod.Tera.neutral");
            ExternalAnimation miniAnimation = animation_registry.LookupAnimation("Teratto.TeraMod.Tera.mini");
            
            var start_cards = new Type[] { typeof(TeraCardTariff), typeof(TeraCardRefund)};
            var playable_birdnerd_character = new ExternalCharacter("Teratto.TeraMod.Tera", TeraDeck!, tera_spr, start_cards, Array.Empty<Type>(), neutralAnimation, miniAnimation);
            playable_birdnerd_character.AddNameLocalisation("Tera");
            playable_birdnerd_character.AddDescLocalisation("<c=tera>TERA</c>\nA tax collector. His cards <c=keyword>debuff</c> enemies, while also <c=keyword>debuffing</c> yourself.");
            registry.RegisterCharacter(playable_birdnerd_character);
        }

        public static string ConditionalGlossaryKey = "";

        public void LoadManifest(IGlossaryRegisty registry)
        {
            ExternalSprite conditionalIcon = ExternalSprite.GetRaw((int)Spr.icons_questionMark);
            ExternalGlossary conditionalGlossary = new ExternalGlossary("Teratto.TeraMod.Conditional", "Conditional", false, ExternalGlossary.GlossayType.cardtrait, conditionalIcon);
            conditionalGlossary.AddLocalisation("en", "Conditional", "The card can be played only when the enemy has enough tax that can be spent.");
            registry.RegisterGlossary(conditionalGlossary);
            ConditionalGlossaryKey = conditionalGlossary.Head;
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            {
                var spr = sprite_registry!.LookupSprite("Teratto.Teramod.EarlyBird");
                var artifact = new ExternalArtifact("Teratto.TeraMod.EarlyBird", typeof(Artifacts.TeraArtifactEarlyBird), spr, new ExternalGlossary[0], deck_registry!.LookupDeck("Teratto.TeraMod.Tera"), null);
                artifact.AddLocalisation("EARLY BIRD", "At the start of combat, gain a <c=card>Gets the Worm</c>.");
                registry.RegisterArtifact(artifact);
            }
            {
                var spr = sprite_registry!.LookupSprite("Teratto.Teramod.Capitalism");
                var artifact = new ExternalArtifact("Teratto.TeraMod.Capitalism", typeof(Artifacts.TeraArtifactCapitalism), spr, new ExternalGlossary[0], deck_registry!.LookupDeck("Teratto.TeraMod.Tera"), null);
                artifact.AddLocalisation("CAPITALISM", "At the start of every turn, gain <c=energy>1 energy</c>. <c=downside>Gain one tax a turn (caps at 3)</c>.");
                registry.RegisterArtifact(artifact);
            }
            {
                var spr = sprite_registry!.LookupSprite("Teratto.Teramod.YearlyPayments");
                var artifact = new ExternalArtifact("Teratto.TeraMod.YearlyPayments", typeof(Artifacts.TeraArtifactYearlyPayments), spr, new ExternalGlossary[0], deck_registry!.LookupDeck("Teratto.TeraMod.Tera"), null);
                artifact.AddLocalisation("YEARLY PAYMENTS", "At the start of combat, apply two <c=status>tax</c> to the enemy.");
                registry.RegisterArtifact(artifact);
            }
            { 
                var spr = sprite_registry!.LookupSprite("Teratto.Teramod.Grant");
                var artifact = new ExternalArtifact("Teratto.TeraMod.Grant", typeof(Artifacts.TeraArtifactGovernmentGrants), spr, new ExternalGlossary[0], deck_registry!.LookupDeck("Teratto.TeraMod.Tera"), null);
                artifact.AddLocalisation("GOVERNMENT GRANT", "At the start of combat, gain 2 <c=status>bailout</c>.");
                registry.RegisterArtifact(artifact);
            }
            {
                var spr = sprite_registry!.LookupSprite("Teratto.Teramod.FlightTraining");
                var artifact = new ExternalArtifact("Teratto.TeraMod.FlightTraining", typeof(Artifacts.TeraArtifactFlightTraining), spr, new ExternalGlossary[0], deck_registry!.LookupDeck("Teratto.TeraMod.Tera"), null);
                artifact.AddLocalisation("FLIGHT TRAINING", "Gain 1 <c=status>evade</c> every 3 turns.");
                registry.RegisterArtifact(artifact);
            }
            {
                var spr = sprite_registry!.LookupSprite("Teratto.Teramod.TeraInflation");
                var artifact = new ExternalArtifact("Teratto.TeraMod.TeraInflation", typeof(Artifacts.TeraArtifactInflation), spr, new ExternalGlossary[0], deck_registry!.LookupDeck("Teratto.TeraMod.Tera"), null);
                artifact.AddLocalisation("INFLATION", "Tax now does <c=damage>1</c> damage for every <c=keyword>2</c> tax a ship has.");
                registry.RegisterArtifact(artifact);
            }



        }

        public void LoadManifest(IStatusRegistry statusRegistry)
        {
            ExternalSprite taxesIcon = sprite_registry!.LookupSprite("Teratto.TeraMod.coin");
            ExternalStatus taxationStatus = new("Teratto.DemoMod.Taxation", false, System.Drawing.Color.Magenta, System.Drawing.Color.DarkMagenta, taxesIcon, affectedByTimestop: false);
            taxationStatus.AddLocalisation("Tax", "At end of turn, deal <c=keyword>1</c> damage for every <c=keyword>3</> tax.");
            statusRegistry.RegisterStatus(taxationStatus);

            TeraModStatuses.Taxation = (Status)taxationStatus.Id!;

            //
            // Note: No longer registering our own missing status, as Nickel will make one for us.
            //
            
            // ExternalSprite missingTeraSprite = sprite_registry!.LookupSprite("Teratto.Teramod.MissingTera");
            // ExternalStatus missingTeraStatus = new("Teratto.DemoMod.MissingTera", false, System.Drawing.Color.Magenta, System.Drawing.Color.DarkMagenta, missingTeraSprite, affectedByTimestop: false);
            // missingTeraStatus.AddLocalisation("Tera is Missing", "The next {0} <c=tera>Tera</c> cards you play do nothing.");
            // statusRegistry.RegisterStatus(missingTeraStatus);
            // TeraModStatuses.MissingTera = (Status)missingTeraStatus.Id!;
            // ExternalDeck teraDeck = deck_registry!.LookupDeck("Teratto.TeraMod.Tera");
            // StatusMeta.deckToMissingStatus[(Deck)teraDeck.Id!] = TeraModStatuses.MissingTera;

            ExternalSprite engineStallNextTurnSprite = sprite_registry!.LookupSprite("Teratto.Teramod.StallNext");
            ExternalStatus engineStallNextTurnStatus = new("Teratto.DemoMod.EngineStallNextTurn", false, System.Drawing.Color.Magenta, System.Drawing.Color.DarkMagenta, engineStallNextTurnSprite, affectedByTimestop: false);
            engineStallNextTurnStatus.AddLocalisation("Engine Stall Next Turn", "<c=downside>Gain one <c=status>engine stall</c> next turn</c>. Decreases by 1 at end of turn.");
            statusRegistry.RegisterStatus(engineStallNextTurnStatus);
            TeraModStatuses.EngineStallNextTurn = (Status)engineStallNextTurnStatus.Id!;

            ExternalSprite engineLockNextTurnSprite = sprite_registry!.LookupSprite("Teratto.Teramod.LockNext");
            ExternalStatus engineLockNextTurnStatus = new("Teratto.DemoMod.EngineLockNextTurn", false, System.Drawing.Color.Magenta, System.Drawing.Color.DarkMagenta, engineLockNextTurnSprite, affectedByTimestop: false);
            engineLockNextTurnStatus.AddLocalisation("Engine Lock Next Turn", "<c=downside>Gain one <c=status>engine lock</c> next turn</c>. Decreases by 1 at end of turn.");
            statusRegistry.RegisterStatus(engineLockNextTurnStatus);
            TeraModStatuses.EngineLockNextTurn = (Status)engineLockNextTurnStatus.Id!;

            ExternalSprite interestSprite = sprite_registry.LookupSprite("Teratto.Teramod.Taxes");
            ExternalStatus interestStatus = new("Teratto.DemoMod.interest", false, System.Drawing.Color.Magenta, System.Drawing.Color.DarkMagenta, interestSprite, affectedByTimestop: false);
            interestStatus.AddLocalisation("Interest", "Gain one <c=status>tax</c> every turn.");
            statusRegistry.RegisterStatus(interestStatus);
            TeraModStatuses.Interest = (Status)interestStatus.Id!;

            ExternalSprite bailoutSprite = sprite_registry.LookupSprite("Teratto.Teramod.BailoutBeta");
            ExternalStatus bailoutStatus = new("Teratto.DemoMod.bailout", true, Colors.status.ToSys(), Colors.statusBorderColor.ToSys(), bailoutSprite, affectedByTimestop: false);
            bailoutStatus.AddLocalisation("Bailout", "The next time you would gain any amount of a <c=status>negative status</c>, instead remove 1 bailout.");
            statusRegistry.RegisterStatus(bailoutStatus);
            TeraModStatuses.Bailout = (Status)bailoutStatus.Id!;


        }
        
        public void LoadManifest(ICustomEventHub eventHub)
        {
            eventHub.ConnectToEvent<Func<IManifest, IPrelaunchContactPoint>>("Nickel::OnAfterDbInitPhaseFinished", HandleAfterDbInitPhaseFinished);
        }
        
        public void FinalizePreperations(IPrelaunchContactPoint prelaunchManifest)
        {
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(DB), nameof(DB.LoadStringsForLocale)),
                postfix: new HarmonyMethod(typeof(ModManifest), nameof(LoadStringsForLocale_Postfix))
            );
        }
        
        private static void LoadStringsForLocale_Postfix(string locale, ref Dictionary<string, string> __result)
        {
            Console.WriteLine("Patching Localization for TeraMod...");
            
            if (locale != "en")
            {
                // We aren't localized ;-;
                return;
            }

            __result.TryAdd($"char.{TeraDeck.Id}.desc.missing", "<c=tera>TERA..?</c>\nTera is missing.");
        }
        
        private void HandleAfterDbInitPhaseFinished(Func<IManifest, IPrelaunchContactPoint> getPcp)
        {
            // Find the missing status that Nickel injected for our character.
            Deck? deck = (Deck?)TeraDeck?.Id;
            if (deck.HasValue) {
                StatusMeta.deckToMissingStatus.TryGetValue(deck.Value, out TeraModStatuses.MissingTera);
            } else {
                Logger?.LogError("TeraDeck does not have a value, cannot find Tera's missing status. This will break things!");
            }
        }
        
        public static Spr GetSprite(string globalName)
        {
            return (Spr)(_instance.sprite_registry!.LookupSprite(globalName).Id ?? 0);
        }
    }
}