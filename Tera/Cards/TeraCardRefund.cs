﻿namespace Tera.Cards
{ //TODO - MAKE THIS INTO A CONDITIONAL CARD

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardRefund : Card
    {
        private int GetRequiredTax()
        {

            int requiredTax = 1;
            return requiredTax;
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            int requiredTax = GetRequiredTax();
           
            int enemyTax = c.otherShip.Get(TeraModStatuses.Taxation);
            if (enemyTax >= requiredTax)
            {
                list.Add(new AStatus()
                {
                    status = TeraModStatuses.Taxation,
                    mode = AStatusMode.Add,
                    statusAmount = -requiredTax,
                    targetPlayer = false
                });
               

            }
            switch (this.upgrade)
            {
                case Upgrade.B:
                    if (enemyTax >= requiredTax)
                    {
                        list.Add(new AStatus()
                        {
                            status = Status.evade,
                            statusAmount = 2,
                            targetPlayer = true

                        });
                        list.Add(new AStatus()
                        {
                            status = Status.tempShield,
                            statusAmount = 2,
                            targetPlayer = true
                        });

                    }
                    break;
                case Upgrade.None:
                    {
                        if (enemyTax >= requiredTax)
                        {
                            list.Add(new AStatus()
                            {
                                status = Status.evade,
                                statusAmount = requiredTax,
                                targetPlayer = true
                            });
                            list.Add(new AStatus()
                            {
                                status = Status.tempShield,
                                statusAmount = 2,
                                targetPlayer = true

                            });
                        }

                    }
                    break;
                case Upgrade.A:
                    {
                        
                        if (enemyTax >= requiredTax)
                        {
                            list.Add(new AStatus()
                            {
                                status = Status.evade,
                                statusAmount = requiredTax,
                                targetPlayer = true
                            });
                            list.Add(new AStatus()
                            {
                                status = Status.tempShield,
                                statusAmount = 3,
                                targetPlayer = true

                            });

                        }

                    }
                    break;

            }
            

            return list;
        }

        public override CardData GetData(State state)
        {
            string desc = "<c=downside>Spend 1 enemy tax</c> to gain 1 <c=status>evade</c> and 2 <c=status>temp shield</c>.";
            if (upgrade == Upgrade.A)
            {
                desc = "<c=downside>Spend 1 enemy tax</c> to gain 1 <c=status>evade</c> and 3 <c=status>temp shield</c>.";

            }
            else if (upgrade == Upgrade.B)
            {
                desc = "<c=downside>Spend 1 enemy tax</c> to gain 2 <c=status>evade</c> and 2 <c=status>temp shield</c>.";

            }
            int requiredTax = GetRequiredTax();
            int currentTax = 0;


            if (state.route is Combat combat)
            {
                currentTax = combat.otherShip.Get(TeraModStatuses.Taxation);
            }
            int cost = 0;
            if (upgrade == Upgrade.B)
            {
                cost = 1;
            }

            return new CardData()
            {

                description = desc,
                cost = cost,
            };
        }

        public override string Name()
        {
            return "TeraCardRefund";
        }
    }
}
