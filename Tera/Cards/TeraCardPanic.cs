﻿namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardPanic : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ADrawCard()
                    {
                        count = 4
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true,
                        //dialogueSelector = ".TeraLeftLOL"
                    });
                    break;
           case Upgrade.A:
                    list.Add(new ADrawCard()
                    {
                        count = 6
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true,
                        //dialogueSelector = ".TeraLeftLOL"
                    });
                    break;

                case Upgrade.B:
                    list.Add(new ADrawCard()
                    {
                        count = 9
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 2,
                        targetPlayer = true,
                        //dialogueSelector = ".TeraLeftLOL"
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
                exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardPanic";
        }
    }
}
