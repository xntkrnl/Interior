using BepInEx.Configuration;
using Dusk;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Interior
{
    internal class HabitatConfig : ContentHandler<HabitatConfig>
    {
        //internal static ConfigEntry<float> lampChance;
        //internal static ConfigEntry<bool> enableForceField;
        //internal static ConfigEntry<bool> enableApparatusMachineSequence;
        //internal static ConfigEntry<bool> enableThreatScannerDoors; //future use

        internal HabitatAssets habitatAssets;
        internal HabitatApparatusAssets apparatusAssets;

        public class HabitatAssets(DuskMod mod, string filePath) : AssetBundleLoader<HabitatAssets>(mod, filePath) { }
        public class HabitatApparatusAssets(DuskMod mod, string filePath) : AssetBundleLoader<HabitatApparatusAssets>(mod, filePath) { }

        public HabitatConfig(DuskMod mod) : base(mod)
        {
            RegisterContent("habitat_dungeondef", out habitatAssets);
            RegisterContent("habitat_apparatusdef", out apparatusAssets);
            //lampChance = habitatAssets.GetConfig<float>("Lamp disable chance");
            //enableForceField = habitatAssets.GetConfig<bool>("Enable forcefields");
        }
    }
}
