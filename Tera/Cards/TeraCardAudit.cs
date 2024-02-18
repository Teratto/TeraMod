namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardAudit : Card
    {
        private int GetTaxAmnt(State s, Combat c)
        {
            int num = 0;
            if (s.route is Combat)
            {
                num = c.otherShip.Get(TeraModStatuses.Taxation);
            }

            return num;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AVariableHint()
                    {
                        status = TeraModStatuses.Taxation,
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 1,
                        targetPlayer = false

                    });

                    break;

                case Upgrade.A:
                    list.Add(new AVariableHint()
                    {
                        status = TeraModStatuses.Taxation,
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 2,
                        targetPlayer = false

                    });
                    break;

                case Upgrade.B:
                    list.Add(new AVariableHint()
                    {
                        status = TeraModStatuses.Taxation,
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetTaxAmnt(s, c),
                        xHint = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Set,
                        statusAmount = 3,
                        targetPlayer = false

                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
          
            return new CardData()
            {
                exhaust = upgrade == Upgrade.B,
                cost = 1,
                art = Spr.cards_GoatDrone,
            };
        }

        public override string Name()
        {
            return "TeraCardPayment";
        }
    }
}
