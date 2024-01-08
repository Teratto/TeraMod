using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod
{
    public interface ITeraModCard
    {
        List<Tooltip> GetExtraTooltips(State s);
    }

    public static class TeraModCardInterfacePatch
    {
        public static void Apply(Harmony harmony, ILogger logger)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Card), nameof(Card.GetAllTooltips)),
                postfix: new HarmonyMethod(AccessTools.Method(typeof(TeraModCardInterfacePatch), nameof(Card_GetAllTooltips_Postfix)))
            );
        }

        public static void Card_GetAllTooltips_Postfix(Card __instance, G g, State s, bool showCardTraits, ref IEnumerable<Tooltip> __result)
        {
            if (__instance is not ITeraModCard teraCard)
            {
                return;
            }

            List<Tooltip> extraTooltips = teraCard.GetExtraTooltips(s);

            if (__result is List<Tooltip> resultList)
            {
                resultList.AddRange(extraTooltips);
            }
            else
            {
                __result = __result.Concat(extraTooltips);
            }
        }
    }
}
