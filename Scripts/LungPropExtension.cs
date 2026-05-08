using Dawn;
using Dawn.Utils;
using System.Collections;
using UnityEngine;

namespace Interior.Scripts
{
    public class LungPropExtension : LungProp
    {
        [Space(10f)]
        public AudioSource? apparatusAudio;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Plugin.mls.LogInfo("LungPropExtension spawned!");
        }
    }
}
