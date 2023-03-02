using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToTarget : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 3.0f;
    protected override void OnStart() {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.Target.transform.position;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.agent.pathPending)
        {
            context.gameObject.GetComponent<enemyAnimController>().SetMoving(true);
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            context.gameObject.GetComponent<enemyAnimController>().SetMoving(false);
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            context.gameObject.GetComponent<enemyAnimController>().SetMoving(false);
            return State.Failure;
        }

        return State.Running;
    }
}
