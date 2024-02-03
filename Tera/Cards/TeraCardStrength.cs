using CobaltCoreModding.Definitions.ExternalItems;
using Tera.Actions;
using Tera.StatusPatches;
using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Cards
{ //STRENGTH SHOULD GIVE 1 ENERGY AND REMOVE 1 TAX. UPGRADE A DRAWS 1 CARD, WHILE UPGRADE B GIVES TWO ENERGY AND REMOVES 2 TAX.

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardStrength : Card
    {
        private int GetRequiredTax()
        {

            int requiredTax = 1;
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
                    targetPlayer = false,
                    dialogueSelector = "TeraStrengthSpeak"
                });
                list.Add(new AEnergy()
                {
                    changeAmount = 2
                });
            }

            if (enemyTax >= requiredTax && upgrade == Upgrade.A)
                {
                list.Add(new ADrawCard()
                {
                    count = 1
                });
                }

            return list;
        }

        public override CardData GetData(State state)
        {
            string desc = "<c=downside>Spend 1 enemy tax</c> to gain <c=status>2 energy</c>.";
            if (upgrade == Upgrade.A)
            {
                desc = "<c=downside>Spend 1 enemy tax</c> to gain 2 <c=status>energy</c> and draw 1 card.";

            }
            else if (upgrade == Upgrade.B)
            {
                desc = "<c=downside>Spend 1 enemy tax</c> to gain <c=status>2 energy</c>.";

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
                retain = upgrade == Upgrade.B ? true : false,
                unplayable = currentTax < requiredTax,
                cost = 0,
                art = new Spr?(Spr.cards_GoatDrone),
            };
        }

        public override string Name()
        {
            return "TeraCardRefund";
        }
    }
}
