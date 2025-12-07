using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interior
{
    internal class Config
    {
        internal static ConfigEntry<float> lampChance;
        internal static ConfigEntry<bool> enableForceField;
        internal static ConfigEntry<bool> enableApparatusMachineSequence;
        internal static ConfigEntry<bool> enableThreatScannerDoors; //future use

        internal static void CreateConfig(ConfigFile cfg)
        {
            enableForceField = cfg.Bind("HabitatInterior", "Enable force fields", true);
            enableApparatusMachineSequence = cfg.Bind("HabitatInterior", "Enable Apparatus Machine Levers", true);
            lampChance = cfg.Bind("HabitatInterior", "Chance for lamp to be turned off from the start", 25f);
        }
    }
}
