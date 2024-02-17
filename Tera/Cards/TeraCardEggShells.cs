namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { }, dontOffer = true)]
    public class TeraCardEggShells : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        fast = true,
                        stunEnemy = true,
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
                art = Spr.cards_GoatDrone,
                exhaust = true,
                temporary = true,
            };
        }

        public override string Name()
        {
            return "TeraCardEggShells";
        }
    }
}
