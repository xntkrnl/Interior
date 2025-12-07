using HarmonyLib;
using Interior.Scripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interior.Patches
{
    //TODO: add events to dawnlib
    internal class EntranceTeleportPatch
    {
        [HarmonyPrefix, HarmonyPatch(typeof(EntranceTeleport), nameof(EntranceTeleport.TeleportPlayerServerRpc))]
        internal static void TeleportPlayerServerRpcPatch(EntranceTeleport __instance)
        {
            if (!__instance.NetworkManager.IsServer) return;
            if (__instance.isEntranceToBuilding) SunHider.onDungeonEntered.Invoke();
            else SunHider.onDungeonExited.Invoke();
        }
    }
}
