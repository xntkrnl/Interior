using BepInEx;
using BepInEx.Logging;
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
        public const string modName = "HabitatInterior";
        public const string modVersion = "1.1.0";

        public static Plugin Instance = null!;
        internal static ManualLogSource mls = null!;
        internal static readonly Harmony harmony = new Harmony(modGUID);

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            mls = Logger;

            NetcodePatcher();
            harmony.PatchAll(typeof(CentipedePatch));
            mls.LogInfo($"{modName} Interior Plugin loaded!");
        }
        private static void NetcodePatcher()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }
    }
}
