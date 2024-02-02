using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Artifacts
{
    public class TeraArtifactYearlyPayments: Artifact
    {
        public override void OnCombatStart(State state, Combat combat)
        {
            combat.QueueImmediate(new AStatus()
            {
                status = TeraModStatuses.Taxation,
                targetPlayer = false,
                statusAmount = 2
            });
        }
    }
}
