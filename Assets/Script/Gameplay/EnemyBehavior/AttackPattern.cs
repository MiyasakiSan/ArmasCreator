using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace ArmasCreator.Behavior
{
    [CreateAssetMenu(fileName = "New Attack Pattern", menuName = "AttackPattern")]
    public class AttackPattern : ScriptableObject
    {
        [SerializeField]
        private int maxActiveCount;
        public int MaxActiveCount => maxActiveCount;

        [SerializeField]
        private float activeDistance;
        public float ActiveDistance => activeDistance;

        [SerializeField]
        private float minActiveDistance;
        public float MinActiveDistance => minActiveDistance;

        [SerializeField]
        private float attackDistance;
        public float AttackDistance => attackDistance;

        [SerializeField]
        private Vector3 activeDirection;
        public Vector3 ActiveDirection => activeDirection;

        [SerializeField]
        private float activeAngleoffset;
        public float ActiveAngleOffset => activeAngleoffset;

        [SerializeField]
        private float damage;
        public float Damage => damage;

        [SerializeField]
        private float attackCooldown;
        public float AttackCooldown => attackCooldown;

        [SerializeField]
        private bool isFollow;
        public bool IsFollow => isFollow;

        [SerializeField]
        private bool isEnrageFinishMove;
        public bool IsEnrageFinishMove => isEnrageFinishMove;

        [SerializeField]
        private string hurtBoxName;
        public string HurtBoxName => hurtBoxName;

        [SerializeField]
        private AnimationClip attackAnimaiton;
        public AnimationClip AttackAnimaiton => attackAnimaiton;

        [SerializeField]
        private float attackDuration;
        public float AttackDuration => attackDuration;

    }
}

