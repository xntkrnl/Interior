using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace Interior
{
    internal class Patches
    {
        static Animator? sunAnimator;
        static Transform? sunTransform;
        static MeshRenderer? sunMeshRenderer;

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

        [HarmonyPostfix, HarmonyPatch(typeof(EntranceTeleport), nameof(EntranceTeleport.TeleportPlayer))]
        static void TeleportPlayerPostifx()
        {
            if (TimeOfDay.Instance == null)
            {
                Plugin.mls.LogError("TimeOfDay is null???");
                return;
            }

            if (sunAnimator == null) sunAnimator = TimeOfDay.Instance.sunAnimator;
            if (sunTransform == null) sunTransform = TimeOfDay.Instance.sunAnimator.transform.Find("SunTexture");

            if (sunTransform == null)
            {
                Plugin.mls.LogWarning("Cant find SunTexture");
                return;
            }

            if (sunMeshRenderer == null) sunMeshRenderer = sunTransform?.GetComponent<MeshRenderer>();

            if (sunMeshRenderer != null) sunMeshRenderer.enabled = !StartOfRound.Instance.localPlayerController.isInsideFactory;
            else Plugin.mls.LogWarning("Cant find SunTexture MeshRenderer");

            //i wish there was other way without checking every single step for null
        }

        [HarmonyPrefix, HarmonyPatch(typeof(animatedSun), nameof(animatedSun.Start))]
        static void StartPatch()
        {
            sunAnimator = null;
            sunTransform = null;
            sunMeshRenderer = null;
        }
    }
}
