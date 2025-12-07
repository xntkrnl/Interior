using Dawn.Utils;
using Discord;
using Unity.Netcode;
using UnityEngine;

namespace Interior.Scripts
{
    internal class LampsRandomizer : NetworkBehaviour
    {
        //i feel like im doing something wrong when doing 1 netvariable for each lamp with 100+ lamps in total.. oh well, whatever.
        public Animator? lampAnimator;
        private float chance;
        private NetworkVariable<bool> on = new NetworkVariable<bool>(true);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (lampAnimator == null || chance <= 0) return;

            if (NetworkManager.IsServer)
            {
                chance = Config.lampChance.Value;
                System.Random random = new System.Random();
                on.Value = random.NextFloat(0, 100) <= chance;
            }

            lampAnimator.SetBool("on", on.Value);
            if (!on.Value) Plugin.mls.LogDebug($"Lamp: {lampAnimator.gameObject.name}, state: {on.Value}, position: {lampAnimator.transform.position}");
        }
    }
}
