using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using ArmasCreator.Utilities;

public class WaitAttackCooldown : ActionNode
{
    public float speed = 1;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 5.0f;

    private Coroutine cooldownCoroutine;
    private CoroutineHelper coroutineHelper;

    private bool isOnCooldown;

    protected override void OnStart() {

        if (blackboard.CurrentAttackPattern != null)
        {
            context.agent.stoppingDistance = stoppingDistance;
            context.agent.speed = speed;
            context.agent.updateRotation = updateRotation;
            context.agent.acceleration = acceleration;

            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
            cooldownCoroutine = coroutineHelper.Play(AttackCooldown(blackboard.CurrentAttackPattern.AttackCooldown));
        }   
    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() 
    {
        if (isOnCooldown)
        {
            context.gameObject.GetComponent<enemyAnimController>().SetWalking(true);
            context.agent.Move(context.transform.forward * Time.deltaTime);
            return State.Running;
        }
        else
        {
            context.agent.isStopped = true;
            context.gameObject.GetComponent<enemyAnimController>().SetWalking(false);
            return State.Success;
        }
    }

    IEnumerator AttackCooldown(float cooldown)
    {
        isOnCooldown = true;

        yield return new WaitForSeconds(cooldown);

        isOnCooldown = false;

        coroutineHelper.Stop(AttackCooldown(blackboard.CurrentAttackPattern.AttackCooldown));

        cooldownCoroutine = null;
    }
}
