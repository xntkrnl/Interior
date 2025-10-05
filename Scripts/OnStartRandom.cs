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
             if (Randomizer.Instance.rnd.Next(100) + 1 <= onStartChance)
                onStart.Invoke();
        }
    }
}
