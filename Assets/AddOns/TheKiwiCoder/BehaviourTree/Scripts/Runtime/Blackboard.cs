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
        public bool isInit;

        [Header("Boss Status")]
        public bool IsDead;
        public bool IsStun;
        public bool IsEnrage;
        public bool IsUseEnrageMode;
        public bool IsPlayerEnterStage;
        public bool canUseEnrageFinishMove;
        public bool canUseRoar;
        public bool IsRunToAttackPos;
        public bool IsAttacking;
        public bool IsFinishRun;

        public float HpPercentage; // Max : 1  ||  Min : 0
        public bool IsMapAttackAvaliable;

        [Header("Attack Pattern")]
        public AttackPattern CurrentAttackPattern;

        public AttackPattern PrevioustAttackPattern;
    }
}