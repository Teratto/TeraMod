using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using DemoMod.StatusPatches;
using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{ //TODO - MAKE THIS INTO A CONDITIONAL CARD

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardRefund : Card
    {
        private int GetRequiredTax()
        {

            int requiredTax = 1;
            if (upgrade == Upgrade.B)
            {
                requiredTax = 2;

            }
            return requiredTax;
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            int requiredTax = GetRequiredTax();
           
            int enemyTax = c.otherShip.Get(TeraModStatuses.Taxation);
            if (enemyTax >= requiredTax)
            {
                list.Add(new AStatus()
                {
                    status = TeraModStatuses.Taxation,
                    mode = AStatusMode.Add,
                    statusAmount = -requiredTax,
                    targetPlayer = false
                });
                list.Add(new AStatus()
                {
                    status = Status.evade,
                    statusAmount = requiredTax,
                    targetPlayer = true
                });
                list.Add(new AStatus()
                {
                    status = upgrade == Upgrade.A ? Status.shield : Status.tempShield,
                    statusAmount = requiredTax,
                    targetPlayer = true
                });
      
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            string desc = "<c=downside>Spend enemy tax once</c> to gain one <c=status>temp shield</c> and one <c=status>evade</c>.";
            if (upgrade == Upgrade.A)
            {
                desc = "<c=downside>Spend enemy tax once</c> to gain one <c=status>shield</c> and one <c=status>evade</c>.";

            }
            else if (upgrade == Upgrade.B)
            {
                desc = "<c=downside>Spend enemy tax twice</c> to gain two <c=status>temp shield</c> and two <c=status>evade</c>.";

            }
            int requiredTax = GetRequiredTax();
            int currentTax = 0;

            if (state.route is Combat combat)
            {
                currentTax = combat.otherShip.Get(TeraModStatuses.Taxation);
            }

            
            return new CardData()
            {
                description = desc,
                unplayable = currentTax < requiredTax,
                cost = 1,
                art = new Spr?(Spr.cards_GoatDrone),
            };
        }

        public override string Name()
        {
            return "TeraCardRefund";
        }
    }
}
