﻿using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardSalesTax: Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TaxationStatusPatches.TaxationStatus,
                        statusAmount = 1,
                        targetPlayer = false

                    });
                    break;

                case Upgrade.A:

                    list.Add(new AStatus()
                    {
                        status = TaxationStatusPatches.TaxationStatus,
                        statusAmount = 1,
                        targetPlayer = false
                    });
                    list.Add(new AStatus()
                    {

                        status = Status.tempShield,
                        statusAmount = 1,
                        targetPlayer = true

                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = TaxationStatusPatches.TaxationStatus,
                        statusAmount = 2,
                        targetPlayer = false
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            int cost = 0;
            return new CardData()

            {
                cost = cost,
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = upgrade == Upgrade.B
            };
        }
        public override string Name()
        {
            return "TeraCardDesperation";
        }
    }
}