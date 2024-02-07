﻿using CobaltCoreModding.Definitions.ExternalItems;
using Tera.Actions;
using Tera.StatusPatches;
using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Cards
{

    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardAllIn : Card
    {
       

        public override List<CardAction> GetActions(State s, Combat c)
        {
            int playerenergy = c.energy;
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Add,
                        statusAmount = playerenergy,
                        targetPlayer = false
                    });

                    list.Add(new AEnergy()
                    {
                        changeAmount = -playerenergy
                    });
                    break;
                case Upgrade.A:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Add,
                        statusAmount = playerenergy,
                        targetPlayer = false
                    });

                    list.Add(new AEnergy()
                    {
                        changeAmount = -playerenergy
                    });
                    break;
               case Upgrade.B:
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.Taxation,
                        mode = AStatusMode.Add,
                        statusAmount = playerenergy,
                        targetPlayer = false
                    });

                    list.Add(new AEnergy()
                    {
                        changeAmount = -2
                    });
                    break;
            }
            

            return list;
        }

        public override CardData GetData(State state)
        {

            string desc = "Apply <c=status>tax</c> equal to your current energy. <c=downside>Lose all energy</c>.";
            if (upgrade == Upgrade.A)
            {
                desc = "Apply <c=status>tax</c> equal to your current energy. <c=downside>Lose all energy</c>.";

            }
            else if (upgrade == Upgrade.B)
            {
                desc = "Apply <c=status>tax</c> equal to your current energy. <c=downside>Lose 2 energy</c>.";

            }

            return new CardData()
            {
                description = desc,
                recycle = upgrade == Upgrade.A ? true : false,
                cost = 0,
                art = new Spr?(Spr.cards_GoatDrone),
            };
        }

        public override string Name()
        {
            return "TeraCardRefund";
        }
    }
}