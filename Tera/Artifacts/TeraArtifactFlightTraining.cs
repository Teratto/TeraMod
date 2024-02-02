using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.Artifacts
{
    internal class TeraArtifactFlightTraining : Artifact
    {
        private int currentTurn;

        public override void OnTurnStart(State state, Combat combat)
        {
            currentTurn++;
            if (currentTurn >= 3)
            {
                combat.QueueImmediate(new AStatus()
                {
                    statusAmount = 1,
                    status = Status.evade,
                    targetPlayer = true
                });
                currentTurn = 0;
            }
        }

        public override int? GetDisplayNumber(State s)
        {
            return currentTurn;
        }


    }
}
