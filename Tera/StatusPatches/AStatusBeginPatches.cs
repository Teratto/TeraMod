using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace Tera.StatusPatches
{
    public class AStatusBeginPatches
    {

        public static void Apply(Harmony harmony, ILogger logger)
        {

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(AStatus), nameof(AStatus.Begin)),
                prefix: new HarmonyMethod(typeof(AStatusBeginPatches), nameof(AStatus_Begin_Prefix))
            );
        }

        public static bool AStatus_Begin_Prefix(AStatus __instance, G g, State s, Combat c)
        {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            if (ship == null || ship.hull <= 0)
            {
                // Run original code. (Which runs this same check eventually, possibly after some other logic, and exits early)
                return true;
            }


            if (__instance.mode != AStatusMode.Add)
            {
                // Run original code.
                return true;
            }

            int currentValue = ship.Get(__instance.status);
            bool isBad = !DB.statuses[__instance.status].isGood && __instance.status != TeraModStatuses.Bailout;
            int currentBailout = ship.Get(TeraModStatuses.Bailout); 

            if (__instance.statusAmount > 0 && currentBailout > 0 && isBad) {
                string? dialogueSelector = null;
                if (__instance.targetPlayer) {
                    dialogueSelector = ".TeraBailedUsOut";
                    switch (__instance.status) {
                        case Status.heat:
                            dialogueSelector += "Heat";
                            break;
                    }
                }
                
                // Remove 1 bailout from the ship.
                c.QueueImmediate(new AStatus()
                {
                    targetPlayer = __instance.targetPlayer,
                    statusAmount = -1,
                    mode = AStatusMode.Add,
                    status = TeraModStatuses.Bailout,
                    dialogueSelector = dialogueSelector
                });

                // Return now and don't run original code.
                return false;
            }

            // We want an immediate shout if Tera goes missing (not wait until end of turn like the others)
            if (__instance.status == TeraModStatuses.MissingTera)
            {
                Narrative.SpeakBecauseOfAction(g, c, ".TeraLeftLOL");
            }

            return true;
        }

        public static void AStatus_Begin_Postfix(AStatus __instance, G g, State s, Combat c)
        {
            if (
                !__instance.targetPlayer 
                && __instance.status == TeraModStatuses.Taxation 
                && __instance.mode == AStatusMode.Add 
                && __instance.statusAmount > 0
            )
            {
                Narrative.SpeakBecauseOfAction(g, c, ".EnemyGotATax");
            }
            
        }

    }
}
