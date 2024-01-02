﻿using CobaltCoreModding.Definitions.ExternalItems;
using DemoMod.Actions;
using DemoMod.StatusPatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] {}, dontOffer = true)]
    public class TeraCardWorm : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
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
           
            return new CardData()

            {
                temporary = true,
                exhaust = true,
                retain = true,
                cost = 0,
                art = new Spr?(Spr.cards_GoatDrone),
  
            };
        }
        public override string Name()
        {
            return "TeraCardWorm";
        }
    }
}
