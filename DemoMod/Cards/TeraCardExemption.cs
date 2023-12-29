using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardExemption : Card
    {
        private int GetTaxAmnt(State s, Combat c)
        {
            int num = 0;
            if (s.route is Combat)
            {
                num = c.otherShip.Get(TaxationStatusPatches.TaxationStatus);
            }

            return num;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = GetTaxAmnt(s, c),
                        targetPlayer = true
                    });

                    break;

                case Upgrade.A:
                    list.Add(new AAttack() { damage = 0, fast = true, stunEnemy = true });
                    list.Add(new AMove()
                    {
                        dir = -1,
                        targetPlayer = true

                    });
                    list.Add(new AAttack() { damage = 0, fast = true, stunEnemy = true });
                    list.Add(new ADrawCard() { count = 1 });
                    break;

                case Upgrade.B:
                    list.Add(new AAttack() { damage = 2, fast = true, stunEnemy = true });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = 1,
                art = new Spr?(Spr.cards_GoatDrone),
                //exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardExemption";
        }
    }
}
