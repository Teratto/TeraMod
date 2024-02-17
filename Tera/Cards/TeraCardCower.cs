namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardCower : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 3,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {

                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraSaysAReallyBadThingAndIAmSorryForWhatHeSaidToYou"

                    });
                    break;

                case Upgrade.A:

                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 3,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {

                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true

                    });
                    list.Add(new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true,
                        dialogueSelector = ".TeraSaysAReallyBadThingAndIAmSorryForWhatHeSaidToYou"

                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 3,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {

                        status = Status.drawLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraSaysAReallyBadThingAndIAmSorryForWhatHeSaidToYou"

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
                art = Spr.cards_GoatDrone,
                //exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardCower";
        }
    }
}
