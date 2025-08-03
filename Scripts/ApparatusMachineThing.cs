using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Interior.Scripts
{
    public class ApparatusMachineThing : NetworkBehaviour
    {
        static public ApparatusMachineThing Instance = null!;
        public LungPropExtension lungProp = null!;

        public int leverCount = 4;
        public Animator animator = null!;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Instance = this;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            Instance = null!;
        }

        public void SendLeverOpen()
        {
            SendLeverOpenServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendLeverOpenServerRpc()
        {
            leverCount--;

            if (leverCount == 0)
                SendLeverOpenClientRpc();
        }

        [ClientRpc]
        public void SendLeverOpenClientRpc()
        {
            animator.SetTrigger("OpenShell");

            if (lungProp != null)
            {
                lungProp.grabbable = true;
                lungProp.grabbableToEnemies = true;
            }
            else Plugin.mls.LogError("Something wrong with apparatus machine/apparatus script!!! Report that to creator of this interior please.");
        }
    }
}
