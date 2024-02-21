using HarmonyLib;
using Microsoft.Extensions.Logging;
using Tera.Artifacts;

namespace Tera.StatusPatches
{

    public static class TaxationStatusPatches
    {
        public static void Apply(Harmony harmony, ILogger logger)
        {
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.OnAfterTurn)),
                postfix: new HarmonyMethod(typeof(TaxationStatusPatches), nameof(Ship_OnAfterTurn_Postfix))
            );
        }

        private static void Ship_OnAfterTurn_Postfix(Ship __instance, State s, Combat c)
        {
            bool inflationGet = s.EnumerateAllArtifacts().Find(a => a.GetType() == typeof(TeraArtifactInflation)) != null;  
            int taxationStatusAmmount = __instance.Get(TeraModStatuses.Taxation);
            int bigTaxTime = 3;
           
            if (inflationGet)
            {
                bigTaxTime = 2;
            }

            if (taxationStatusAmmount >= bigTaxTime)
            {
                bool isPlayer = __instance == s.ship;
                int hurtAmmount = taxationStatusAmmount / bigTaxTime;

                string dialogueSelector = isPlayer ? ".TeraTaxHurtUs" : ".TeraTaxHurtEnemy";
                if (hurtAmmount >= 5) {
                    dialogueSelector += "ALot";
                }
                
                c.QueueImmediate(new AHurt()
                {
                    hurtShieldsFirst = true,
                    hurtAmount = hurtAmmount,
                    targetPlayer = isPlayer,
                    dialogueSelector = dialogueSelector,
                    whoDidThis = (Deck)ModManifest.TeraDeck!.Id!.Value
                });
                
            }
        }

    }
}
