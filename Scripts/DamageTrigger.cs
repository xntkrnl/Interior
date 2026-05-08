using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Interior.Scripts
{
    internal class KillTrigger : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            var player = other.GetComponent<PlayerControllerB>();
            if (player != StartOfRound.Instance.localPlayerController) return;
            
            player.KillPlayer(Vector3.zero, true, CauseOfDeath.Gravity, 0);
        }
    }
}
