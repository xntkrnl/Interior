using Dawn.Utils;
using Discord;
using Interior.Helpers;
using Unity.Netcode;
using UnityEngine;

namespace Interior.Scripts
{
    internal class LampsRandomizer : NetworkBehaviour
    {
        public Animator? lampAnimator;
        [Range(0f, 100f)]
        public float onStartChance;
        private NetworkVariable<bool> on = new NetworkVariable<bool>(true);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (lampAnimator == null) return;

            if (NetworkManager.IsServer)
            {
                System.Random random = new System.Random();
                on.Value = random.NextBool();
            }

            lampAnimator.SetBool("on", on.Value);
            Plugin.mls.LogInfo($"Lamp: {lampAnimator.gameObject.name}, state: {on.Value}, position: {lampAnimator.transform.position}");
        }
    }
}
