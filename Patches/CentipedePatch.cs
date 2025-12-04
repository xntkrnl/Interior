using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace Interior.Patches
{
    //funny thing: they will stop render at max distance.
    internal class CentipedePatch
    {
        [HarmonyTranspiler, HarmonyPatch(typeof(CentipedeAI), nameof(CentipedeAI.RaycastToCeiling))]
        static IEnumerable<CodeInstruction> RaycastToCeilingPatch(IEnumerable<CodeInstruction> instructions)
        {
            var cm = new CodeMatcher(instructions);
            cm.MatchForward(false, new CodeMatch(OpCodes.Ldc_R4, 20f)).SetInstruction(new CodeInstruction(OpCodes.Ldc_R4, 80f));

            //foreach (var instruction in cm.Instructions())
            //    Plugin.mls.LogInfo(instruction);

            return cm.InstructionEnumeration();
        }

        [HarmonyTranspiler, HarmonyPatch(typeof(CentipedeAI), nameof(CentipedeAI.fallFromCeiling), MethodType.Enumerator)]
        static IEnumerable<CodeInstruction> FallFromCeilingPatch(IEnumerable<CodeInstruction> instructions)
        {
            var cm = new CodeMatcher(instructions);
            cm.MatchForward(false, new CodeMatch(OpCodes.Ldc_R4, 20f)).SetInstruction(new CodeInstruction(OpCodes.Ldc_R4, 80f));

            return cm.InstructionEnumeration();
        }
    }
}
