using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToAttackPos : ActionNode
{
    public float speed = 5;

    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 5.0f;

    public bool disable;
    private enemyAnimController animController;

    protected override void OnStart() 
    {
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        context.agent.speed = speed;

        animController = context.gameObject.GetComponent<enemyAnimController>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (disable) { return State.Success; }

        if (blackboard.CurrentAttackPattern != null && !blackboard.IsRunToAttackPos)
        {
            context.agent.isStopped = false;

            context.agent.stoppingDistance = blackboard.CurrentAttackPattern.AttackDistance;
            context.agent.destination = blackboard.Target.transform.position;

            animController.SetMoving(true);
            blackboard.IsRunToAttackPos = true;
            return State.Running;
        }

        if (blackboard.IsRunToAttackPos)
        {
            context.agent.stoppingDistance = blackboard.CurrentAttackPattern.AttackDistance;
            context.agent.destination = blackboard.Target.transform.position;

            if (context.agent.remainingDistance < blackboard.CurrentAttackPattern.AttackDistance)
            {
                Debug.Log(context.agent.remainingDistance);

                context.agent.isStopped = true;
                blackboard.IsRunToAttackPos = false;
                animController.SetMoving(false);
                animController.ResetAllLookAt();
                return State.Success;
            }

            animController.SetMoving(true);
            animController.SetAllLookAtWeight(1);
            return State.Running;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            context.agent.isStopped = true;
            animController.SetMoving(false);
            return State.Failure;
        }
        else
        {
            Debug.Log(blackboard.CurrentAttackPattern);

            animController.SetMoving(false);
            return State.Success;
        }
    }
}
