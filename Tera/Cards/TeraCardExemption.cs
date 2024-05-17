namespace Tera.Cards
{
    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardExemption : Card
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
            switch (upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        statusAmount = -1,
                        targetPlayer = false
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = GetTaxAmnt(s, c) ,
                        targetPlayer = true,
                        mode = AStatusMode.Add 
                    });
                   
                    break;

                case Upgrade.A:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = GetTaxAmnt(s, c) - 1,
                        targetPlayer = true,
                        mode = AStatusMode.Add
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = GetTaxAmnt(s, c),
                        targetPlayer = true,
                        mode = AStatusMode.Add
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            string desc = "<c=downside>Remove one tax</c>, then gain <c=status>shield</c> equal to enemy's <c=status>tax</c>.";
            if(upgrade == Upgrade.A)
            {
                desc = "Gain <c=status>shield</c> equal to enemy's <c=status>tax</c>, minus one.";

            }
            else if(upgrade == Upgrade.B)
            {
                desc = "Gain <c=status>temp shield</c> equal to enemy's <c=status>tax</c>.";

            }
            return new CardData()
            {
                description = desc,
                cost = 1,
                //exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardExemption";
        }
    }
}
