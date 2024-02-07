using CobaltCoreModding.Definitions.ExternalItems;
using Tera.Actions;
using Tera.StatusPatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardAudit : Card
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

                case Upgrade.A:
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 2,
                        targetPlayer = false

                    });
                    break;

                case Upgrade.B:
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 3,
                        targetPlayer = false

                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            string desc = "Attack for dmg equal to enemy's <c=status>tax.</c> <c=downside>Set enemy tax to 1</c>.";
            if (upgrade == Upgrade.A)
            {
                desc = "Attack for dmg equal to enemy's <c=status>tax.</c> <c=downside>Set enemy tax to 2</c>.";

            }
            else if (upgrade == Upgrade.B)
            {
                desc = "Attack for dmg equal to enemy's <c=status>tax.</c> <c=downside>Set enemy tax to 3</c>.";

            }
                return new CardData()
            {
                exhaust = upgrade == Upgrade.B ? true : false,
                description = desc,
                cost = 1,
                art = new Spr?(Spr.cards_GoatDrone),
            };
        }

        public override string Name()
        {
            return "TeraCardPayment";
        }
    }
}
