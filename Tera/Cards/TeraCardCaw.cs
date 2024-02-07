﻿using CobaltCoreModding.Definitions.ExternalItems;
using Tera.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardCaw : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAttack() { damage = GetDmg(s, 2), fast = true });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Bailout,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraBreakoutSpeak"
                    });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = Status.energyLessNextTurn,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraBreakoutSpeak"
                    });
                    list.Add(new AAttack() { damage = GetDmg(s, 2), fast = true });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Bailout,
                        statusAmount = 2,
                        targetPlayer = true

                    });
                  
                    break;

                case Upgrade.A:
                    list.Add(new AAttack() { damage = GetDmg(s, 3), fast = true });
                    list.Add(new AStatus()
                    {

                        status = TeraModStatuses.Bailout,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraBreakoutSpeak"
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = 2,
                art = new Spr?(Spr.cards_GoatDrone),
                 
            };
        }

        public override string Name()
        {
            return "TeraCardCaw";
        }
    }
}
