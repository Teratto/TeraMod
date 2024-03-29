﻿namespace Tera.Cards
{
    //TO DO: Use the missingTera Status in this card
    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraMarketCrash : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        statusAmount = 2,
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true,
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

                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        statusAmount = 2
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    break;

                case Upgrade.B:
                list.Add(new AStatus()
                {
                    status = TeraModStatuses.Taxation,
                    statusAmount = 3
                });
                list.Add(new AStatus()
                {
                    status = Status.tempShield,
                    statusAmount = 4,
                    targetPlayer = true
                });
                list.Add(new AStatus()
                {
                    status = TeraModStatuses.MissingTera,
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
                cost = 2,
            };
        }

        public override string Name()
        {
            return "TeraCardMarketCrash";
        }
    }
}
