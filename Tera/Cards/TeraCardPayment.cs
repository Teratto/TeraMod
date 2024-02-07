﻿using CobaltCoreModding.Definitions.ExternalItems;
using Tera.Actions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { }, dontOffer = true)]
    public class TeraCardPayment : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                      status = TeraModStatuses.Bailout,
                      statusAmount = 1,
                      targetPlayer = true,
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
                art = new Spr?(Spr.cards_GoatDrone),
                exhaust = true,
                temporary = true,
            };
        }

        public override string Name()
        {
            return "TeraCardPayment";
        }
    }
}
