namespace Tera.Cards
{ 

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardFraud : Card
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

            int damageToDeal = 1;

            if (enemyTax >= requiredTax)
            {
                list.Add(new AStatus()
                {
                    status = TeraModStatuses.Taxation,
                    mode = AStatusMode.Add,
                    statusAmount = -requiredTax,
                    targetPlayer = false
                });
                damageToDeal = upgrade == Upgrade.A ? 4 : 3;
            }

            list.Add(new AAttack()
            {
                damage = GetDmg(s,damageToDeal),
            });

            if (enemyTax >= requiredTax && upgrade == Upgrade.B)
            {
                list.Add(new AStatus()
                {
                    status = Status.shield,
                    statusAmount = 1,
                    targetPlayer = true
                });
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            string desc = "Shoot for <c=attack>1</c> dmg. Spend one enemy <c=status>tax</c> to shoot for <c=attack>3</c> dmg instead.";
            if (upgrade == Upgrade.A)
            {
                desc = "Shoot for <c=attack>1</c> dmg. Spend one enemy <c=status>tax</c> to shoot for <c=attack>4</c> dmg instead.";

            }
            else if (upgrade == Upgrade.B)
            {
                desc = "Shoot for <c=attack>1</c> dmg. Spend one <c=status>tax</c> to shoot for <c=attack>3</c> dmg and gain <c=status>1 shield</c>.";

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
                cost = 1,
                art = Spr.cards_GoatDrone,
            };
        }

        public override string Name()
        {
            return "TeraCardRefund";
        }
    }
}
