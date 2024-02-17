namespace Tera.Cards
{
    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardBailout : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Bailout,
                        statusAmount = 1,
                        targetPlayer = true

                    });
                    break;

                case Upgrade.A:

                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Bailout,
                        statusAmount = 2,
                        targetPlayer = true
                    });

                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true,
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Bailout,
                        statusAmount = 1,
                        targetPlayer = true
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
                cost = 1;
            }
            return new CardData()
            {
                cost = cost,
                art = Spr.cards_GoatDrone,
                exhaust = upgrade != Upgrade.B
            };
        }
        public override string Name()
        {
            return "TeraCardDesperation";
        }
    }
}
