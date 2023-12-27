using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardCower : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 3,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {

                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = false

                    });
                    break;

                case Upgrade.A:

                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 4,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {

                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = false

                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 3,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 3,
                        targetPlayer = true

                    });
                    list.Add(new AStatus()
                    {

                        status = Status.overdrive,
                        statusAmount = 2,
                        targetPlayer = false

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
                //exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardEgg";
        }
    }
}
