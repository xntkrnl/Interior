using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Dawn.Utils;
using Dusk;
using HarmonyLib;
using Interior.Patches;
using System.Reflection;
using UnityEngine;

namespace Interior
{
    [BepInPlugin(modGUID, modName, modVersion)]
    internal class Plugin : BaseUnityPlugin
    {
        public const string modGUID = "mborsh.HabitatIntreior";
        public const string modName = "Habitat Interior";
        public const string modVersion = "1.2.0";

        public static Plugin Instance = null!;
        internal static ManualLogSource mls = null!;
        internal static readonly Harmony harmony = new Harmony(modGUID);
        internal static DuskMod mod = null!;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            mls = Logger;
            AssetBundle mainBundle = AssetBundleUtils.LoadBundle(Assembly.GetExecutingAssembly(), "habitat_contentcontainer");
            mod = DuskMod.RegisterMod(this, mainBundle);
            mod.RegisterContentHandlers();

            harmony.PatchAll(typeof(CentipedePatch));
            harmony.PatchAll(typeof(EntranceTeleportPatch));
            harmony.PatchAll(typeof(RoundManagerPatch));
            mls.LogInfo($"{modName} Plugin loaded!");
        }
    }
}
