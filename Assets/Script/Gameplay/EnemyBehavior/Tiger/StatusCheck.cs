using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class StatusCheck : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (!CheckCurrentStatus())
        {
            return State.Failure;
        }
        else
        {
            return State.Running;
        }
    }

    private bool CheckCurrentStatus()
    {
        Debug.Log(context.gameObject);
        var enemyStat = context.gameObject.GetComponent<EnemyStat>();

        if(enemyStat == null) { return false; }

        float maxHealth = enemyStat.MaxHealth;
        float currentHealth = enemyStat.CurrentHealth;
        float healthRatio = currentHealth / maxHealth;

        blackboard.HpPercentage = healthRatio;

        // TODO : need to implement check is Stun and is Enrage

        return true;
    }
}
