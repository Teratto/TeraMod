using DemoMod.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Artifacts
{
    public class TeraArtifactEarlyBird: Artifact
    {
        public override void OnCombatStart(State state, Combat combat)
        {
            combat.QueueImmediate(new AAddCard()
            {
                card = new TeraCardWorm(),
                destination = CardDestination.Hand,
                amount = 1,
            });
        }

        public override List<Tooltip>? GetExtraTooltips()
        {
            List<Tooltip> tooltips = new List<Tooltip>();
            tooltips.Add(new TTCard()
            {
                card = new TeraCardWorm(),
                showCardTraitTooltips = true
            });
            return tooltips;
        }        
    }
}
