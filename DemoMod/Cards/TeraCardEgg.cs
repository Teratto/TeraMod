using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{
    
    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardEgg : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAttack() { damage = GetDmg(s, 0), fast = true , stunEnemy = true});
                    break;
               
                case Upgrade.A:
                    list.Add(new AAttack() { damage = GetDmg(s, 0), fast = true, stunEnemy = true });
                    list.Add(new ADrawCard() { count = 1});
                    break;

                case Upgrade.B:
                    list.Add(new AAttack() {damage = GetDmg(s, 2), fast = true, stunEnemy = true });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = 0,
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = true 
                //exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardEgg";
        }
    }
}
