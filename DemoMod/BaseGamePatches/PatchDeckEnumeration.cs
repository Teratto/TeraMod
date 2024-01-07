using System;
using System.Reflection.Emit;
using System.Reflection;
using HarmonyLib;
using Microsoft.Extensions.Logging;


namespace DemoMod.BaseGamePatches
{
    public static class PatchDeckEnumeration
    {
        private static ILogger? _logger;

        public static void Apply(Harmony harmony, ILogger logger)
        {
            _logger = logger;

            HarmonyMethod transpiler = new(typeof(PatchDeckEnumeration), nameof(GetActionsTranspiler));

            // Patch the GetAction method of every card class.
            Type cardType = typeof(Card);
            foreach (Type type in cardType.Assembly.GetTypes())
            {
                if (!type.IsAssignableTo(cardType))
                {
                    continue;
                }

                MethodInfo? getActionsMethod = type.GetMethod(nameof(Card.GetActions), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                if (getActionsMethod == null)
                {
                    continue;
                }

                harmony.Patch(original: getActionsMethod, transpiler: transpiler);
            }
        }

        private static IEnumerable<CodeInstruction> GetActionsTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
        {
            List<CodeInstruction> instructionBuffer = new();

            using IEnumerator<CodeInstruction> enumerator = instructions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                CodeInstruction instruction = enumerator.Current;
                instructionBuffer.Add(instruction);

                TryInterceptDeckBug(enumerator, instructionBuffer, original);

                // Emit everything in the buffer.
                foreach (CodeInstruction bufferedInstruction in instructionBuffer)
                {
                    yield return bufferedInstruction;
                }
                instructionBuffer.Clear();
            }
        }

        private static void TryInterceptDeckBug(IEnumerator<CodeInstruction> instructionEnumerator, List<CodeInstruction> instructionBuffer, MethodBase original)
        {
            // Intercept code which looks like the following:
            // ldtoken      Deck
            // call         class [System.Runtime]System.Type [System.Runtime]System.Type::GetTypeFromHandle(valuetype [System.Runtime]System.RuntimeTypeHandle)
            // call         class [System.Runtime]System.Array [System.Runtime]System.Enum::GetValues(class [System.Runtime]System.Type)

            CodeInstruction currentIns = instructionEnumerator.Current;

            if (currentIns.opcode != OpCodes.Ldtoken)
            {
                return;
            }

            Type operandType = (Type)currentIns.operand;
            if (operandType != typeof(Deck))
            {
                return;
            }

            if (!instructionEnumerator.MoveNext())
            {
                return;
            }
            currentIns = instructionEnumerator.Current;
            instructionBuffer.Add(currentIns);

            if (currentIns.opcode != OpCodes.Call)
            {
                return;
            }

            MethodInfo getTypeFromHandleMethod = AccessTools.Method(typeof(Type), "GetTypeFromHandle",
                new Type[] {
                    typeof(RuntimeTypeHandle)
                }
            )!;
            if ((MethodInfo)currentIns.operand != getTypeFromHandleMethod)
            {
                return;
            }

            if (!instructionEnumerator.MoveNext())
            {
                return;
            }
            currentIns = instructionEnumerator.Current;
            instructionBuffer.Add(currentIns);

            if (currentIns.opcode != OpCodes.Call)
            {
                return;
            }
            MethodInfo operandMethodInfo = (MethodInfo)currentIns.operand;
            if (!(operandMethodInfo.DeclaringType == typeof(Enum) && operandMethodInfo.Name == nameof(Enum.GetValues)))
            {
                return;
            }

            // We have a hit! Replace the instruction buffer with our desired instruction to be emit instead.
            instructionBuffer.Clear();
            instructionBuffer.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PatchDeckEnumeration), nameof(PatchedDeckGetValues))));

            _logger?.LogInformation("Patched card {CardName}", original.DeclaringType?.FullName);
        }

        private static Deck[] PatchedDeckGetValues()
        {
            // Use the DB instead of Enum.GetValues!!
            return DB.decks.Keys.ToArray();
        }
    }
}
