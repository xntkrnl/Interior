using Interior.Helpers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interior.Scripts
{
    internal class OnStartRandom : MonoBehaviour
    {
        public UnityEvent? onStart;
        [Range(0f, 100f)]
        public float onStartChance;

        void Start()
        {
            ServerRandomizer.Instance.NextServerRpc(100, out int result);
            if (result + 1 <= onStartChance)
                onStart.Invoke();
        }
    }
}
