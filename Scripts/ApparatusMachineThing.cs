using DunGen;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            if (NetworkManager.IsHost)
                StartCoroutine(WaitUntilLanding());
        }

        IEnumerator WaitUntilLanding()
        {
            //i dont remember why this waiting exist nor why is it trying to check if its true because its true until ship starting landing
            yield return new WaitUntil(() => StartOfRound.Instance.inShipPhase == true);

            lungProp = GameObject.FindObjectsOfType<LungPropExtension>().FirstOrDefault(lp => Vector3.Distance(this.transform.position, lp.transform.position) <= 20f);

            if (!lungProp)
            {
                Plugin.mls.LogError("Something wrong with apparatus machine/apparatus script!!! Report that to creator of this interior please.");
                Plugin.mls.LogError("Host was unable to find new apparatus");
                yield break;
            }

            SendLungPropClientRpc(lungProp);
        }

        [ClientRpc]
        private void SendLungPropClientRpc(NetworkBehaviourReference lpr)
        {
            if (!lpr.TryGet(out LungPropExtension lungPropFromServer))
            {
                Plugin.mls.LogError("Something wrong with apparatus machine/apparatus script!!! Report that to creator of this interior please.");
                Plugin.mls.LogError("Something went wrong on recieving lung prop on ClientRpc");
                return;
            }

            lungProp = lungPropFromServer;

            lungProp.grabbable = false;
            lungProp.grabbableToEnemies = false;

            //if (NetworkManager.IsHost && HabitatConfig.enableApparatusMachineSequence.Value) ApparatusMachineThing.Instance.SendLeverOpenClientRpc();

            lungProp.isLungDocked = true;
            lungProp.isLungPowered = true;

            if (lungProp.apparatusAudio != null)
            {
                lungProp.apparatusAudio.loop = true;
                lungProp.apparatusAudio.Play();
            }
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
