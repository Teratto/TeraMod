namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] {Upgrade.A, Upgrade.B}, dontOffer = true)]
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
                case Upgrade.A:
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s, 2),
                        fast = true,
                        stunEnemy = true,
                    });
                    break;
                case Upgrade.B:
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        fast = true,
                        stunEnemy = true,
                    });
                    list.Add(new AMove()
                    {
                        dir = 2,
                        targetPlayer = true

                    });
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
                exhaust = true,
                temporary = true,
                flippable = upgrade == Upgrade.B,
            };
        }

        public override string Name()
        {
            return "TeraCardEggShells";
        }
    }
}
