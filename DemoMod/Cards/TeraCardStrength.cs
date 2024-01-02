using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardStrength : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAttack()
                    {
                        damage = 0,
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 1,
                        targetPlayer = true

                    });
                    list.Add(new AEnergy()
                    {
                        changeAmount = 1
                    });

                    break;

                case Upgrade.B:
                    list.Add(new AAttack()
                    {
                        damage = 0,
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 1,
                        targetPlayer = true

                    });
                    list.Add(new AEnergy()
                    {
                        changeAmount = 1
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AAttack()
                    {
                        damage = 0,
                        status = TeraModStatuses.Taxation,
                        statusAmount = 2
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 1,
                        targetPlayer = true

                    });
                    list.Add(new AEnergy()
                    {
                        changeAmount = 1
                    });

                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            int cost = 1;
              if (upgrade == Upgrade.B)
            {
                cost = 0;
            }

            return new CardData()
            {
                cost = cost,
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = true
            };
        }

        public override string Name()
        {
            return "TeraCardStrength";
        }
    }
}
