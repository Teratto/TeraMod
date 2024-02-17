namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardHealthInsurance : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AHeal()
                    {
                       healAmount = 1,
                       targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AHeal()
                    {
                        healAmount = 2,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AHeal()
                    {
                        healAmount = 4,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 2,
                        targetPlayer = true
                    });
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
                exhaust = upgrade == Upgrade.B ? false : true,
                singleUse = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardHealthInsurance";
        }
    }
}
