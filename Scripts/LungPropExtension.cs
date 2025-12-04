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
        /*public static EnemyType? oldBirdEnemyType
        {
            get
            {
                if (field == null)
                    oldBirdEnemyType = OriginalContent.Enemies.Find(enemy => string.CompareOrdinal(enemy.enemyName, "RadMech") == 0);

                return field;
            }

            private set;
        }*/

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            Plugin.mls.LogInfo("LungPropExtension spawned!");

            if (!StartOfRound.Instance.inShipPhase)
                StartCoroutine(WaitAndDoSmth());
        }

        IEnumerator WaitAndDoSmth()
        {
            yield return new WaitUntil(() => ApparatusMachineThing.Instance != null);
            Plugin.mls.LogInfo("ApparatusMachineThing instance found!");
            ApparatusMachineThing.Instance.lungProp = this;

            grabbable = false;
            grabbableToEnemies = false;

            if (NetworkManager.IsHost && Config.enableApparatusMachineSequence.Value) ApparatusMachineThing.Instance.SendLeverOpenClientRpc();

            isLungDocked = true;
            isLungPowered = true;

            //radMechEnemyType = oldBirdEnemyType;

            if (apparatusAudio != null)
            {
                apparatusAudio.loop = true;
                apparatusAudio.Play();
            }
        }
    }
}
