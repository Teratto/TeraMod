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

    [CardMeta(deck = Deck.test, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardTaxRun : Card
    {
       
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
                   
                    list.Add(new AMove()
                    {
                        dir = 2,
                        targetPlayer = true
                        
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraLeftLOL"
                    });
                    break;

                case Upgrade.A:
                  
                    list.Add(new AMove()
                    {
                        dir = 2,
                        targetPlayer = true
                    });
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraLeftLOL"
                    });

                    break;

                case Upgrade.B:
                  
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
                    });
                    list.Add(new AMove()
                    {
                        dir = 2,
                        targetPlayer = true

                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 1,
                        targetPlayer = true,
                        dialogueSelector = ".TeraLeftLOL"
                    });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                flippable = (upgrade == Upgrade.A ? true : false),
                cost = 0,
                art = new Spr?(Spr.cards_GoatDrone),
            };
        }
        public override string Name()
        {
            return "TeraCardTaxRun";
        }
    }
}
