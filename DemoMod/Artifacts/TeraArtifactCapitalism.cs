using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod.Artifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class TeraArtifactCapitalism : Artifact
    {
        public override void OnTurnStart(State state, Combat combat)
        {
            int playerTax = state.ship.Get(TeraModStatuses.Taxation);

            if (playerTax < 3)
            {
                combat.QueueImmediate(new AStatus()
                {
                    status = TeraModStatuses.Taxation,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            }

        }
        public override void OnReceiveArtifact(State state)
        {
            state.ship.baseEnergy++;
        }

        public override void OnRemoveArtifact(State state)
        {
            state.ship.baseEnergy--;
        }
    }
}
