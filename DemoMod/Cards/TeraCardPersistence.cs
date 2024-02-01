using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using DemoMod.StatusPatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardPersistence : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Interest,
                        statusAmount = 1,
                        targetPlayer = false,
                        dialogueSelector = "TeraPersistenceSpeak"

                    });
                    break;

                case Upgrade.A:

                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Interest,
                        statusAmount = 1,
                        targetPlayer = false,
                        dialogueSelector = "TeraPersistenceSpeak"
                    });

                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Interest,
                        statusAmount = 1,
                        targetPlayer = false,
                        dialogueSelector = "TeraPersistenceSpeak"
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        statusAmount = 2,
                        targetPlayer = false
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            int cost = 3;
            if(this.upgrade == Upgrade.A)
            {
                cost = 2;
            }
            return new CardData()

            {
                cost = cost,
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = true
            };
        }
        public override string Name()
        {
            return "TeraCardDesperation";
        }
    }
}
