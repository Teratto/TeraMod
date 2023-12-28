using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{
    //TO DO: Use the missingTera Status in this card
    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraMarketCrash : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TaxationStatusPatches.TaxationStatus,
                        statusAmount = 2
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 3,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.missingBooks,
                        statusAmount = 1
                    });
                    break;

                case Upgrade.A:

                    list.Add(new AStatus()
                    {
                        targetPlayer = true,
                        status = Status.evade,
                        statusAmount = 1
                    });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 1
                    });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 1
                    });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 1
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        targetPlayer = true,
                        status = Status.evade,
                        statusAmount = 2
                    });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 1
                    });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 2
                    });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 1
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = 0,
                art = new Spr?(Spr.cards_GoatDrone),
                flippable = upgrade == Upgrade.A
            };
        }

        public override string Name()
        {
            return "TeraCardMarketCrash";
        }
    }
}
