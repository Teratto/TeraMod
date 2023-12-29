using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{ //TODO - MAKE THIS INTO A CONDITIONAL CARD

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardRefund : Card
    {
        private int GetTaxAmnt(State s, Combat c)
        {
            int num = 0;
            if (s.route is Combat)
            {
                num = c.otherShip.Get(TaxationStatusPatches.TaxationStatus);
            }

            return num;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TaxationStatusPatches.TaxationStatus,
                        mode = AStatusMode.Add,
                        statusAmount = -1,
                        targetPlayer = false
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 1,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 1,
                        targetPlayer = true
                    });

                    break;

                case Upgrade.A:
                    list.Add(new AStatus()
                    {
                        status = TaxationStatusPatches.TaxationStatus,
                        mode = AStatusMode.Add,
                        statusAmount = -1,
                        targetPlayer = false
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
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = TaxationStatusPatches.TaxationStatus,
                        mode = AStatusMode.Add,
                        statusAmount = -2,
                        targetPlayer = false
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 2,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
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
            };
        }

        public override string Name()
        {
            return "TeraCardRefund";
        }
    }
}
