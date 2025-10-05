using Interior.Helpers;
using UnityEngine;

namespace Interior.Scripts
{
    internal class LampsRandomizer : MonoBehaviour
    {
        public Animator? lampAnimator;
        [Range(0f, 100f)]
        public float onStartChance;

        void Start()
        {
            if (lampAnimator == null) return;

            if (Randomizer.Instance.rnd.Next(100) + 1 <= onStartChance)
            {
                lampAnimator.SetBool("on", false);
                Plugin.mls.LogInfo($"Disabling lamp: {lampAnimator.gameObject.name}, with chance: {onStartChance} at {lampAnimator.transform.position}");
            }
        }
    }
}
