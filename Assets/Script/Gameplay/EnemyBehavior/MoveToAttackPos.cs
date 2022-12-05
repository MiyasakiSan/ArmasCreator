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

    protected override void OnStart() 
    {
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        context.agent.speed = speed;
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

            context.gameObject.GetComponent<enemyAnimController>().MovingServerRpc(true);
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
                context.gameObject.GetComponent<enemyAnimController>().MovingServerRpc(false);
                return State.Success;
            }

            context.gameObject.GetComponent<enemyAnimController>().MovingServerRpc(true);
            return State.Running;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            context.agent.isStopped = true;
            context.gameObject.GetComponent<enemyAnimController>().MovingServerRpc(false);
            return State.Failure;
        }
        else
        {
            Debug.Log(blackboard.CurrentAttackPattern);

            context.gameObject.GetComponent<enemyAnimController>().MovingServerRpc(false);
            return State.Success;
        }
    }
}
