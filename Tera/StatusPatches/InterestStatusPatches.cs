using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tera.StatusPatches
{
    public static class InterestStatusPatches
    {
        public static void Apply(Harmony harmony, ILogger logger)
        {
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.OnBeginTurn)),
                postfix: new HarmonyMethod(typeof(InterestStatusPatches), nameof(Ship_OnBeginTurn_Postfix))
            );
        }

        private static void Ship_OnBeginTurn_Postfix(Ship __instance, State s, Combat c)
        {
            int interest = __instance.Get(TeraModStatuses.Interest);

            if (interest > 0)
            {
                __instance.Add(TeraModStatuses.Taxation, interest);
            }
        }
    }
}
