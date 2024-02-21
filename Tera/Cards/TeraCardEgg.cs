namespace Tera.Cards
{
    
    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardEgg : Card
    {
        public bool isTemporary;
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            
            bool useSpecialDialogue = s.rngScript.Next() < .01;  // 1 / 100 chance
            const string SpecialSelector = "TeraPlayedASpecialEgg";
            
            switch (upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAttack() 
                    { 
                        damage = GetDmg(s, 0),
                        fast = true,
                        stunEnemy = true,
                        dialogueSelector = useSpecialDialogue ? SpecialSelector : ".TeraPlayedAnEgg"
                    });
                    break;
               
                case Upgrade.A:
                    list.Add(new AAttack() 
                    { 
                        damage = GetDmg(s, 0),
                        fast = true,
                        stunEnemy = true,
                        dialogueSelector =  useSpecialDialogue ? SpecialSelector : ".TeraPlayedAnEgg"
                    });
                    list.Add(new ADrawCard()
                    {
                        count = 1
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AAttack() 
                    {
                        damage = GetDmg(s, 0),
                        fast = true,
                        stunEnemy = true,
                        dialogueSelector = useSpecialDialogue ? SpecialSelector : ".TeraPlayedAHardboiledEgg"
                    });
                    list.Add(new AAddCard()
                    {
                        card = new TeraCardEggShells()
                        {
                          temporaryOverride = true,
                        },
                        destination = CardDestination.Discard,
                        amount = 1,
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
                retain = upgrade == Upgrade.A,
                //exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardEgg";
        }
    }
}
