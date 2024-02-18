namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardTenacity : Card
    {
        public bool isTemporary;

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAddCard()
                    {
                        card = new TeraCardTenacity()
                        {
                            isTemporary = true
                        },
                        destination = CardDestination.Hand,
                        amount = 1,
                        dialogueSelector = ".teraTenacitySpeak"
                    });
                    list.Add(new ADrawCard()
                    {
                        count = 3
                    });
                    list.Add(new ADiscard()
                    {
                        count = 3
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AAddCard()
                    {
                        card = new TeraCardTenacity()
                        {
                            upgrade = Upgrade.A,
                            isTemporary = true
                        },
                        destination = CardDestination.Hand,
                        amount = 1,
                        dialogueSelector = ".teraTenacitySpeak"
                    });
                    list.Add(new ADiscard()
                    {
                        count = 3
                    });
                    list.Add(new ADrawCard()
                    {
                        count = 3
                    });

                    break;

                case Upgrade.B:
                    list.Add(new ADrawCard()
                    {
                        count = 3,
                        dialogueSelector = ".teraTenacitySpeak"
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            int cost = 0;
            if (this.upgrade == Upgrade.B)
            {
                cost = 1;
            }
            return new CardData()

            {
                temporary = isTemporary,
                cost = cost,
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = upgrade != Upgrade.B,
                infinite = upgrade == Upgrade.B
            };
        }
        public override string Name()
        {
            return "TeraCardTenacity";
        }
    }
}
