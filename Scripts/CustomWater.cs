using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Interior.Scripts
{
    internal class CustomWater : MonoBehaviour
    {
        public float movementHinderance = 0.7f;
        private bool isSinkingLocalPlayer;
        public int audioClipIndex = 1;
        public Collider? waterCollider;

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            var player = other.gameObject.GetComponent<PlayerControllerB>();
            if (player != StartOfRound.Instance.localPlayerController) return;

            if (player.isInElevator || player.isInHangarShipRoom)
            {
                if (isSinkingLocalPlayer) StopSinking(player);
                return;
            }

            player.underwaterCollider = waterCollider;
            player.isUnderwater = true;
            player.statusEffectAudioIndex = audioClipIndex;

            if (GameNetworkManager.Instance.localPlayerController == player && (player.isFallingFromJump || player.isFallingNoJump) && player.fallValue < -4f)
                TimeOfDay.Instance.WaterSplashEffect(player.transform.position, player.fallValue > -17f, syncToServer: true);

            if (player.isSinking) return;

            if (isSinkingLocalPlayer)
            {
                if (!CheckConditions(player)) StopSinking(player);
            }
            else if (CheckConditions(player))
            {
                Plugin.mls.LogInfo("Set local player sinking in custom water to true!");
                //Plugin.mls.LogInfo($"Physics parent = {player.physicsParent.name}");
                isSinkingLocalPlayer = true;
                player.isMovementHindered++;
                player.sourcesCausingSinking++;
                player.hinderedMultiplier *= movementHinderance;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!isSinkingLocalPlayer) return;
            if (!other.gameObject.CompareTag("Player")) return;

            var player = other.gameObject.GetComponent<PlayerControllerB>();
            if (player != StartOfRound.Instance.localPlayerController) return;

            StopSinking(player);
        }

        public bool CheckConditions(PlayerControllerB player)
        {
            //if (player.inSpecialInteractAnimation || player.inAnimationWithEnemy || player.isClimbingLadder) return false;
            //if (player.physicsParent) return false;
            if (player.isInHangarShipRoom || player.isInElevator) return false;
            //if (!player.thisController.isGrounded) return false;

            return true;
        }

        public void StopSinking(PlayerControllerB player)
        {
            Plugin.mls.LogInfo("Set local player in custom water to false!");
            isSinkingLocalPlayer = false;
            player.isMovementHindered = Mathf.Clamp(player.isMovementHindered - 1, 0, 100);
            player.hinderedMultiplier = Mathf.Clamp(player.hinderedMultiplier / movementHinderance, 1f, 100f);
            player.sourcesCausingSinking = Mathf.Clamp(player.sourcesCausingSinking - 1, 0, 100);
            if (player.isMovementHindered == 0) player.isUnderwater = false;
        }
    }
}
