using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace ArmasCreator.Behavior
{
    public struct AttackPattern
    {
        [SerializeField]
        private int activeCount;
        public int ActiveCount => activeCount;

        [SerializeField]
        private float activeDistance;
        public float ActiveDistance => activeDistance;

        [SerializeField]
        private Vector3 activeDirection;
        public Vector3 ActiveDirection => activeDirection;

        [SerializeField]
        private AnimationClip attackAnimaiton;
        public AnimationClip AttackAnimaiton => attackAnimaiton;

        public void ResetActiveCount()
        {
            activeCount = 0;
        }

    }
}

