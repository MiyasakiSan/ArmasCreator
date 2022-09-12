using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsDead : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (isDead())
        {
            return State.Failure;
        }
        else
        {
            return State.Success;
        }
    }
    private bool isDead()
    {
        float currentHealth = context.gameObject.GetComponent<EnemyStat>().CurrentHealth;
        if(currentHealth <= 0)
        {
            context.gameObject.GetComponent<enemyAnimController>().DeadServerRpc();
            return true;
        }
        else
        {
            return false;
        }
    }
}
