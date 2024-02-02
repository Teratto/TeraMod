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

    [CardMeta(deck = Deck.test, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TeraCardTaxRun : Card
    {
       
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();

            switch (this.upgrade)
            {
                case Upgrade.None:
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
                        targetPlayer = true
                    });
                    break;

                case Upgrade.A:
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
                        targetPlayer = true
                    });

                    break;

                case Upgrade.B:
                    list.Add(new AAttack()
                    {
                        damage = GetDmg(s, 0),
                        status = TeraModStatuses.Taxation,
                        statusAmount = 1
                    });
                    list.Add(new AStatus()
                    {
                        status = Status.evade,
                        statusAmount = 2,
                        targetPlayer = true
                    });
                    list.Add(new AStatus()
                    {
                        status = TeraModStatuses.MissingTera,
                        statusAmount = 2,
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
