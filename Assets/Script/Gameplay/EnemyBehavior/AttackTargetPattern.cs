using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackTargetPattern : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (blackboard.CurrentAttackPattern != null) { Debug.Log($"ATTACK TARGET WITH {blackboard.CurrentAttackPattern.AttackAnimaiton.name}"); }

        return State.Success;
    }
}
