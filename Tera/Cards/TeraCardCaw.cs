namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardCaw : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAttack() { damage = GetDmg(s, 2), fast = true });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Bailout,
                        statusAmount = 2,
                        targetPlayer = true,
                        dialogueSelector = ".TeraBreakoutSpeak"
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AAttack() { damage = GetDmg(s, 2), fast = true });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Bailout,
                        statusAmount = 2,
                        targetPlayer = true,
                        dialogueSelector = "TeraBreakoutSpeak"
                    });
                  
                    break;

                case Upgrade.A:
                    list.Add(new AAttack() { damage = GetDmg(s, 2), fast = true });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Bailout,
                        statusAmount = 2,
                        targetPlayer = true,
                        dialogueSelector = ".TeraBreakoutSpeak"
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
                exhaust = upgrade != Upgrade.B,
                 
            };
        }

        public override string Name()
        {
            return "TeraCardCaw";
        }
    }
}
