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
    private enemyAnimController animController;

    private bool isOnCooldown;

    protected override void OnStart() {

        if (blackboard.CurrentAttackPattern != null)
        {
            animController = context.gameObject.GetComponent<enemyAnimController>();

            context.agent.stoppingDistance = stoppingDistance;
            context.agent.speed = speed;
            context.agent.updateRotation = updateRotation;
            context.agent.acceleration = acceleration;
            context.agent.isStopped = false;

            animController.SetAllLookAtWeight(1);

            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();
            cooldownCoroutine = coroutineHelper.Play(AttackCooldown(blackboard.CurrentAttackPattern.AttackCooldown));
        }   
    }

    protected override void OnStop() {

    }

    protected override State OnUpdate() 
    {
        if (blackboard.CurrentAttackPattern != null)
        {
            if (blackboard.CurrentAttackPattern.AttackCooldown == 0)
            {
                coroutineHelper.StopCoroutine(cooldownCoroutine);
                cooldownCoroutine = null;

                return State.Success;
            }
        }

        if (blackboard.IsOnCooldown)
        {
            context.gameObject.GetComponent<enemyAnimController>().SetWalking(true);

            Vector3 dir = blackboard.Target.transform.position - context.transform.position;

            Vector3 pos = new Vector3(blackboard.Target.transform.position.x + dir.magnitude * Mathf.Cos(30 * Mathf.Deg2Rad), 0,
                                      blackboard.Target.transform.position.z + dir.magnitude * Mathf.Sin(30 * Mathf.Deg2Rad));

            context.agent.destination = pos;

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
        blackboard.IsOnCooldown = true;

        yield return new WaitForSeconds(cooldown + 0.25f);

        blackboard.IsOnCooldown = false;

        cooldownCoroutine = null;
    }
}
