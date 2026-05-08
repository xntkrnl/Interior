using HarmonyLib;
using Interior.Scripts;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Interior.Patches
{
    internal class RoundManagerPatch
    {
        //internal static UnityEvent OnAfterSetPowerOffAtStart = new();

        [HarmonyPostfix, HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SetPowerOffAtStart))]
        static void SetPowerOffAtStartPatch()
        {
            //OnAfterSetPowerOffAtStart.Invoke();
            foreach (var lamp in LampsRandomizer.lamps)
                lamp.SetLampState();
            //i wish there was a better solution that i could think of
            //basically i need this patch because my lamps are now networkobjects and execution order of methods is fucked if i will do SetLampState on network spawn
        }

        //comented out for posible future use (does not work atm iirc)
        /*[HarmonyTranspiler, HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnSyncedProps))]
        static IEnumerable<CodeInstruction> SpawnSyncedPropsPatch(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var cm = new CodeMatcher(instructions);

            var spawnMethod = AccessTools.Method(typeof(NetworkObject), "Spawn");

            cm.MatchForward(false, new CodeMatch(i => i.Calls(spawnMethod)));
            var skipLabel = generator.DefineLabel();
            cm.MatchBack(false, new CodeMatch(OpCodes.Ldloc_2));

            var arrayLd = new CodeInstruction(OpCodes.Ldloc_0); //array
            var indexLd = new CodeInstruction(OpCodes.Ldloc_3); //i
            var goLd = new CodeInstruction(OpCodes.Ldloc_3); //gameObject2

            var newInstructions = new List<CodeInstruction>
            {
                //if (array[i] is SpawnSyncedObjectWithScale)
                //  gameObject2.transform.localScale = array[i].transform.localScale;

                arrayLd, indexLd, //array[i]
                new CodeInstruction(OpCodes.Ldelem_Ref),
                new CodeInstruction(OpCodes.Isinst, typeof(SpawnSyncedObjectWithScale)), //is SpawnSyncedObjectWithScale
                new CodeInstruction(OpCodes.Brfalse_S, skipLabel),

                goLd, //gameObject2
                new CodeInstruction(OpCodes.Callvirt, "get_transform"), //gameObject2.transform

                arrayLd, indexLd, //array[i]
                new CodeInstruction(OpCodes.Ldelem_Ref),
                new CodeInstruction(OpCodes.Callvirt, "get_transform"),  //array[i].transform
                new CodeInstruction(OpCodes.Callvirt, "get_localScale"),   //array[i].transform.localScale
                new CodeInstruction(OpCodes.Callvirt, "set_localScale")     //gameObject2.transform.localScale = array[i].transform.localScale
            };

            cm.Insert(newInstructions);
            cm.MatchForward(false, new CodeMatch(i => i.Calls(spawnMethod)));
            cm.Instruction.labels.Add(skipLabel);

            foreach (var inst in cm.Instructions())
                Plugin.mls.LogMessage(inst);

            return cm.InstructionEnumeration();
        }*/
    }
}

