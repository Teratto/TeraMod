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
    public class TeraCardPanic : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ADrawCard()
                    {
                        count = 4
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    break;
           case Upgrade.A:
                    list.Add(new ADrawCard()
                    {
                        count = 6
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    break;

                case Upgrade.B:
                    list.Add(new ADrawCard()
                    {
                        count = 9
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
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
                cost = 1,
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = upgrade == Upgrade.B
            };
        }

        public override string Name()
        {
            return "TeraCardPanic";
        }
    }
}
