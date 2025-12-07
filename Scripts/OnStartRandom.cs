using Dawn.Utils;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Interior.Scripts
{
    internal class OnStartRandom : NetworkBehaviour
    {
        public UnityEvent? onStartClients;
        public UnityEvent? onStartServer;

        [Range(0f, 100f)]
        public float onStartChance;
        private NetworkVariable<float> randomizedInt = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (onStartChance == 0 || (onStartClients.GetPersistentEventCount() == 0 && onStartServer.GetPersistentEventCount() == 0)) return;

            if (IsServer)
            {
                System.Random random = new System.Random();
                randomizedInt.Value = random.NextFloat(0, onStartChance + 1);

                if (randomizedInt.Value <= onStartChance)
                    onStartServer.Invoke();
            }

            if (randomizedInt.Value <= onStartChance)
                onStartClients.Invoke();
        }
    }
}
