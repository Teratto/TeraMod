﻿namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] {Upgrade.A, Upgrade.B}, dontOffer = true)]
    public class TeraCardWorm : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AEnergy()
                    {
                        changeAmount = 1
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AEnergy()
                    {
                        changeAmount = 2
                    });

                    break;

                case Upgrade.B:
                    list.Add(new AEnergy()
                    {
                        changeAmount = 1
                    });
                    list.Add(new ADrawCard()
                    {
                        count = 2
                    });

                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                temporary = true,
                exhaust = true,
                retain = true,
                cost = 0,
            };
        }
        public override string Name()
        {
            return "TeraCardWorm";
        }
    }
}
