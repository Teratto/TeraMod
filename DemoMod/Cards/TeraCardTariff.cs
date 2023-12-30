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
    public class TeraCardTariff : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ATaxingAttack() 
                    { 
                        Tax = 1,
                        damage = 1
                    });
                    break;

                case Upgrade.B:
                    list.Add(new ATaxingAttack()
                    {
                        Tax = 1,
                        damage = 2
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    list.Add(new ATaxingAttack() 
                    { 
                        Tax = 1,
                        damage = 1
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            //int cost = 1;
          //  if (upgrade == Upgrade.B)
            //{
           //     cost = 0;
           // }

            return new CardData()
            {
                cost = 1,
                art = new Spr?(Spr.cards_GoatDrone),

            };
        }

        public override string Name()
        {
            return "TeraCardTariff";
        }
    }
}
