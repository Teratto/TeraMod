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
    public class TeraCardPanic : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 1 
                    });
                    list.Add(new AAttack() { damage = 0, fast = true, stunEnemy = false });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 2
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
                    list.Add(new AAttack() { damage = 0, fast = true, stunEnemy = false });
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 2
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 1
                    });
                    list.Add(new AAttack() { damage = 0, fast = true });
              
                    list.Add(new AMove()
                    {
                        isRandom = true,
                        targetPlayer = true,
                        dir = 2
                    });

                    list.Add(new AAttack() { damage = 0, fast = true });
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
                //flippable = upgrade == Upgrade.A
            };
        }

        public override string Name()
        {
            return "TeraCardPanic";
        }
    }
}
