using CobaltCoreModding.Definitions.ExternalItems;
using Tera.Actions;
using Tera.StatusPatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardTenacity : Card
    {
        public bool isTemporary;

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAddCard()
                    {
                        card = new TeraCardTenacity()
                        {
                            isTemporary = true
                        },
                        destination = CardDestination.Hand,
                        amount = 1,
                        dialogueSelector = "teraTenacitySpeak"
                    });
                    list.Add(new ADrawCard()
                    {
                        count = 2
                    });
                    list.Add(new ADiscard()
                    {
                        count = 3
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AAddCard()
                    {
                        card = new TeraCardTenacity()
                        {
                            upgrade = Upgrade.A,
                            isTemporary = true
                        },
                        destination = CardDestination.Hand,
                        amount = 1,
                        dialogueSelector = "teraTenacitySpeak"
                    });
                    list.Add(new ADiscard()
                    {
                        count = 3
                    });
                    list.Add(new ADrawCard()
                    {
                        count = 2
                    });

                    break;

                case Upgrade.B:
                    list.Add(new ADrawCard()
                    {
                        count = 2,
                        dialogueSelector = "teraTenacitySpeak"
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            int cost = 0;
            if (this.upgrade == Upgrade.B)
            {
                cost = 1;
            }
            return new CardData()

            {
                temporary = isTemporary,
                cost = cost,
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = upgrade == Upgrade.B ? false : true,
                infinite = upgrade == Upgrade.B ? true : false
            };
        }
        public override string Name()
        {
            return "TeraCardTenacity";
        }
    }
}
