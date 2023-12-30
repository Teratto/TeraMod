using DemoMod.Actions;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod
{
    public static class StallAndLockNextTurnStatusPatches
    {
        public static void Apply(Harmony harmony, ILogger logger)
        {
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.OnBeginTurn)),
                postfix: new HarmonyMethod(typeof(StallAndLockNextTurnStatusPatches), nameof(Ship_OnBeginTurn_Postfix))
            );
        }

        private static void Ship_OnBeginTurn_Postfix(Ship __instance, State s, Combat c)
        {
            int engineStallNextTurnValue = __instance.Get(TeraModStatuses.EngineStallNextTurn);
            int engineLockNextTurnValue = __instance.Get(TeraModStatuses.EngineLockNextTurn);

            if (engineStallNextTurnValue > 0)
            {
                __instance.Add(Status.engineStall, 1);
                __instance.Add(TeraModStatuses.EngineStallNextTurn, -1);
            }
            if (engineLockNextTurnValue > 0)
            {
                __instance.Add(Status.lockdown, 1);
                __instance.Add(TeraModStatuses.EngineLockNextTurn, -1);
            }
        }

    }
}
