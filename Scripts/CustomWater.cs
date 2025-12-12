using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Interior.Scripts
{
    internal class CustomWater : MonoBehaviour
    {
        public float movementHinderance = 1.6f;
        private bool isSinkingLocalPlayer;
        public int audioClipIndex;
        public Collider waterCollider;

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

            if (player.isSinking) return;

            if (isSinkingLocalPlayer)
            {
                if (!CheckConditions(player)) StopSinking(player);
            }
            else if (CheckConditions(player))
            {
                Plugin.mls.LogInfo("Set local player to sinking in custom water!");
                isSinkingLocalPlayer = true;
                player.sourcesCausingSinking++;
                player.isMovementHindered++;
                player.hinderedMultiplier *= movementHinderance;
                player.sinkingSpeedMultiplier = 0;
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
            if (player.inSpecialInteractAnimation || player.inAnimationWithEnemy || player.isClimbingLadder) return false;
            if (player.physicsParent) return false;
            if (player.isInHangarShipRoom || player.isInElevator) return false;
            if (!player.thisController.isGrounded) return false;

            return true;
        }

        public void StopSinking(PlayerControllerB player)
        {
            isSinkingLocalPlayer = false;
            player.sourcesCausingSinking = Mathf.Clamp(player.sourcesCausingSinking - 1, 0, 100);
            player.isMovementHindered = Mathf.Clamp(player.isMovementHindered - 1, 0, 100);
            player.hinderedMultiplier = Mathf.Clamp(player.hinderedMultiplier / movementHinderance, 1f, 100f);
            if (player.isMovementHindered == 0) player.isUnderwater = false;
        }
    }
}
