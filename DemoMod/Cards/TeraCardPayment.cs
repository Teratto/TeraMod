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
    public class TeraCardPayment : Card
    {
        private int GetTaxAmnt(State s, Combat c)
        {
            int num = 0;
            if (s.route is Combat)
            {
                num = c.otherShip.Get(TeraModStatuses.Taxation);
            }

            return num;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AVariableHint()
                    {
                        status = TeraModStatuses.Taxation,
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 0,
                        targetPlayer = false

                    });

                    break;

                case Upgrade.A:
                    list.Add(new AVariableHint()
                    {
                        status = TeraModStatuses.Taxation,
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 1,
                        targetPlayer = false

                    });
                    break;

                case Upgrade.B:
                    list.Add(new AVariableHint()
                    {
                        status = TeraModStatuses.Taxation,
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 0,
                        targetPlayer = false

                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
                int cost = 1;
                if (upgrade == Upgrade.B)
                {
                     cost = 0;
                }
                return new CardData()
            {
                cost = cost,
                art = new Spr?(Spr.cards_GoatDrone),
            };
        }

        public override string Name()
        {
            return "TeraCardPayment";
        }
    }
}
