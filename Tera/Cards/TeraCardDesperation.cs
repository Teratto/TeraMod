namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardDesperation : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                       status = Status.powerdrive,
                       statusAmount = 1,
                       targetPlayer = true,
                       dialogueSelector = ".TeraDesperationSpeak"
                    });
                    list.Add(new AStatus()
                    {

                       status = TeraModStatuses.Taxation,
                       statusAmount = 3,
                       targetPlayer = true

                    });
                    break;

                case Upgrade.A:

                    list.Add(new AStatus()
                    {
                        status = Status.powerdrive,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraDesperationSpeak"
                    });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Taxation,
                        statusAmount = 3,
                        targetPlayer = true

                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = Status.powerdrive,
                        statusAmount = 2,
                        targetPlayer = true,
                        dialogueSelector = ".TeraDesperationSpeak"
                    });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Taxation,
                        statusAmount = 6,
                        targetPlayer = true

                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            int cost = 2;
            if (upgrade == Upgrade.A)
            {
                cost = 1;
            }
            return new CardData()
            {
                cost = cost,
                art = Spr.cards_GoatDrone,
                exhaust = true
            };
        }
        public override string Name()
        {
            return "TeraCardDesperation";
        }
    }
}
