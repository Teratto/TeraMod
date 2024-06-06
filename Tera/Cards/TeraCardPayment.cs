namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class TeraCardPayment : Card
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
                      targetPlayer = true,
                    });
                    break;
                case Upgrade.A:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Bailout,
                        statusAmount = 2,
                        targetPlayer = true,
                    });
                    break;
                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Bailout,
                        statusAmount = 1,
                        targetPlayer = true,
                    });
                    list.Add(new ADrawCard()
                    {
                        count = 2,
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = 0,
                exhaust = true,
                temporary = true,
            };
        }

        public override string Name()
        {
            return "TeraCardPayment";
        }
    }
}
