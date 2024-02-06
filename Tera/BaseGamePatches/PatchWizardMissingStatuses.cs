using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;

namespace Tera.BaseGamePatches
{
    public static class PatchWizardMissingStatuses
    {
        public static void Apply(Harmony harmony, ILogger logger)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Wizard), nameof(Wizard.GetAssignableStatuses)),
                prefix: new HarmonyMethod(AccessTools.Method(typeof(PatchWizardMissingStatuses), nameof(Wizard_GetAssignableStatuses_Prefix)))
            );
        }
        
        public static bool Wizard_GetAssignableStatuses_Prefix(State s, ref List<Status>? __result)
        {
            // Wizard's GetAssignableStatuses method doesn't regard the deckToMissingStatus dictionary and instead
            // has it's own Deck to Status lookup. As a result, the stock version of Wizard.GetAssignableStatuses will
            // never return a list containing the missing statuses for mod characters.
            
            if (__result == null)
            {
                __result = new List<Status>();
            }

            foreach (Character character in s.characters)
            {
                Deck? deck = character.deckType;
                Status status = Status.heat;
                if (deck.HasValue)
                {
                    if (StatusMeta.deckToMissingStatus.TryGetValue(deck.Value, out Status foundStatus))
                    {
                        status = foundStatus;
                    }
                }
                __result.Add(status);
            }

            return false;
        }
    }
}
