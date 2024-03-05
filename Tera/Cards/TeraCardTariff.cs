namespace Tera.Cards
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
                    list.Add(new AAttack() 
                    { 
                        damage = GetDmg(s,1),
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s,2),
                        status = TeraModStatuses.Taxation,
                        statusAmount = 2
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    list.Add(new AAttack() 
                    { 
                        damage = GetDmg(s,1),
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
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
                cost = 2;
            }

            return new CardData()
            {
                cost = cost,
            };
        }

        public override string Name()
        {
            return "TeraCardTariff";
        }
    }
}
