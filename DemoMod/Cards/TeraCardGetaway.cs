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
    public class TeraCardGetaway : Card, ITeraModCard
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 2,
                        targetPlayer = true

                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.EngineStallNextTurn,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    break;

                case Upgrade.A:
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 3,
                        targetPlayer = true

                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.EngineStallNextTurn,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 4,
                        targetPlayer = true

                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.EngineLockNextTurn,
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
                //exhaust = upgrade == Upgrade.B
            };
        }

        public List<Tooltip> GetExtraTooltips(State s)
        {
            List<Tooltip> tooltips = new();

            if (upgrade == Upgrade.None || upgrade == Upgrade.A)
            {
                tooltips.Add(new TTGlossary($"status.{Status.engineStall.Key()}", 1));
            }
            else
            {
                tooltips.Add(new TTGlossary($"status.{Status.lockdown.Key()}", 1));
            }

            return tooltips;
        }

        public override string Name()
        {
            return "TeraCardGetaway";
        }
    }
}
