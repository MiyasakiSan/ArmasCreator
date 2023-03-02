using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArmasCreator.Behavior;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {
        [Header("Roaming")]
        public Vector3 moveToPosition;

        public GameObject Target;

        [Header("Boss Status")]
        public bool IsDead;
        public bool IsStun;
        public bool IsEnrage;
        public bool IsPlayerEnterStage;
        public bool IsRunToAttackPos;
        public bool IsAttacking;

        public float HpPercentage; // Max : 1  ||  Min : 0
        public bool IsMapAttackAvaliable;

        [Header("Attack Pattenr")]
        public AttackPattern CurrentAttackPattern;

    }
}