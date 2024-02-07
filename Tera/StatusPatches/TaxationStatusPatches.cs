﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using System.Reflection;
using Tera.Actions;
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

            //harmony.Patch(
            //    original: AccessTools.DeclaredMethod(typeof(AAttack), nameof(AAttack.Begin)),
            //    transpiler: new HarmonyMethod(typeof(TaxationStatusPatches), nameof(AAttack_Begin_Transpiler))
            //);
        }

        private static void Ship_OnAfterTurn_Postfix(Ship __instance, State s, Combat c)
        {
            bool inflationGet = s.artifacts.Find(a => a.GetType() == typeof(TeraArtifactInflation)) != null;  
            int taxationStatusAmmount = __instance.Get(TeraModStatuses.Taxation);
            int bigTaxTime = 3;
           
            if (inflationGet == true)
            {
                bigTaxTime = 2;
            }

            if (taxationStatusAmmount >= bigTaxTime)
            {

                bool isPlayer = __instance == s.ship;

                c.QueueImmediate(new AHurt()
                {
                    hurtShieldsFirst = true,
                    hurtAmount = taxationStatusAmmount / bigTaxTime,
                    targetPlayer = isPlayer
                });
            }
        }

        //private static IEnumerable<CodeInstruction> AAttack_Begin_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
        //{
        //    MethodInfo targetMethod = AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.NormalDamage));
        //    MethodInfo methodToInject = AccessTools.DeclaredMethod(typeof(TaxationStatusPatches), nameof(InterceptAttackDamage));

        //    bool wasAbleToInject = false;

        //    foreach (CodeInstruction ins in instructions)
        //    {
        //        yield return ins;

        //        if (ins.opcode == OpCodes.Callvirt)
        //        {
        //            // It's a CallVirt instruction, is it called Ship::NormalDamage?
        //            MethodInfo calledMethod = (MethodInfo)ins.operand;
        //            if (calledMethod == targetMethod)
        //            {
        //                // Emit a call
        //                yield return new CodeInstruction(OpCodes.Ldarg_0); // this
        //                yield return new CodeInstruction(OpCodes.Ldarg_2); // s
        //                yield return new CodeInstruction(OpCodes.Ldarg_3); // c
        //                yield return new CodeInstruction(OpCodes.Call, methodToInject);
        //                // I'm also assuming that calling a void return type method will not push any result on the the execution stack... Fingers crossed!!

        //                wasAbleToInject = true;
        //            }
        //        }
        //    }

        //    if (!wasAbleToInject)
        //    {
        //        throw new Exception("AAA WASNT ABLE TO INJECT!!!");
        //    }
        //}

        //private static void InterceptAttackDamage(AAttack attackAction, State state, Combat combat)
        //{
        //    Ship targetShip = attackAction.targetPlayer ? state.ship : combat.otherShip;

        //    if (attackAction is ATaxingAttack taxingAttackAction)
        //    {
        //        targetShip.Add(TeraModStatuses.Taxation, taxingAttackAction.Tax);
        //    }
        //}

    }
}