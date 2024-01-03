using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Artifacts
{
    public class TeraArtifactGovernmentGrants : Artifact
    {
        public override void OnCombatStart(State state, Combat combat)
        {
            combat.QueueImmediate(new AStatus()
            {
                status = TeraModStatuses.Bailout,
                targetPlayer = true,
                statusAmount = 1

            });
        }

        public override List<Tooltip>? GetExtraTooltips()
        {
            List<Tooltip> tooltips = new();
            tooltips.Add(new TTGlossary($"status.{TeraModStatuses.Bailout}"));
            return tooltips;
        }

    }
}
