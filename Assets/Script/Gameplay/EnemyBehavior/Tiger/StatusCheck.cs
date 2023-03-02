using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using ArmasCreator.Utilities;

public class StatusCheck : ActionNode
{
    private CoroutineHelper coroutineHelper;

    public float EnrageDuration;
    protected override void OnStart() {
        coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (!CheckCurrentStatus())
        {
            return State.Failure;
        }
        else
        {
            return State.Success;
        }
    }

    private bool CheckCurrentStatus()
    {
        var enemyStat = context.gameObject.GetComponent<EnemyStat>();

        if (enemyStat == null) { return false; }

        float maxHealth = enemyStat.MaxHealth;
        float currentHealth = enemyStat.CurrentHealth;
        float healthRatio = currentHealth / maxHealth;

        blackboard.HpPercentage = healthRatio;

        // TODO : need to implement check is Stun and is Enrage

        if (healthRatio <= 0.40f && !blackboard.IsUseEnrageMode)
        {
            blackboard.IsUseEnrageMode = true;

            coroutineHelper.StartCoroutine(EnrageMode(EnrageDuration));
        }

        return true;
    }

    IEnumerator EnrageMode(float sec)
    {
        blackboard.IsEnrage = true;

        yield return new WaitForSeconds(sec);

        blackboard.IsEnrage = false;
        blackboard.canUseEnrageFinishMove = true;
    }
}
