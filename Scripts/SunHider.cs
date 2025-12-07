using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Interior.Scripts
{
    //im sorta stole it from itolib so uh bruh kill me
    internal class SunHider : MonoBehaviour
    {
        private MeshRenderer? sunTexture;
        internal static UnityEvent onDungeonEntered = new UnityEvent();
        internal static  UnityEvent onDungeonExited = new UnityEvent();
        private bool foundSun = false;

        private void Awake()
        {
            Transform sunTransform = (TimeOfDay.Instance && TimeOfDay.Instance.sunAnimator) ? TimeOfDay.Instance.sunAnimator.transform.Find("SunTexture") : null!;

            foundSun = sunTransform != null && sunTransform.TryGetComponent(out sunTexture);
        }

        private void OnEnable()
        {
            onDungeonEntered.AddListener(HideSun);
            onDungeonExited.AddListener(RevealSun);
            StartOfRound.Instance.playerTeleportedEvent.AddListener(ToggleSun);
        }

        private void OnDisable()
        {
            onDungeonEntered.RemoveListener(HideSun);
            onDungeonExited.RemoveListener(RevealSun);
            StartOfRound.Instance.playerTeleportedEvent.RemoveListener(ToggleSun);
        }

        private void RevealSun()
        {
            if (foundSun == true) sunTexture.enabled = true;
        }

        private void HideSun()
        {
            if (foundSun == true) sunTexture.enabled = false;
        }

        private void ToggleSun(PlayerControllerB player)
        {
            if (!player.isInsideFactory) HideSun();
            else RevealSun();
        }
    }
}
