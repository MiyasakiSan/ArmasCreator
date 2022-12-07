using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    public float DetectedRanged;
    protected override void OnStart() {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (CheckIsPlayerNearBy())
        {
            return State.Failure;

        }
        if (context.agent.pathPending) {
            context.gameObject.GetComponent<enemyAnimController>().SetMoving(true);
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance) {
            context.gameObject.GetComponent<enemyAnimController>().SetMoving(false);
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            context.gameObject.GetComponent<enemyAnimController>().SetMoving(false);
            return State.Failure;
        }

        return State.Running;
    }
    public bool CheckIsPlayerNearBy()
    {
        List<GameObject> PlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        GameObject nearestTarget = GetClosestEnemy(PlayerList, context.transform);
        if (Vector3.Distance(context.gameObject.transform.position, nearestTarget.transform.position) < DetectedRanged)
        {
            if (blackboard.Target != nearestTarget)
            {
                blackboard.Target = nearestTarget;
                Debug.Log("Enemy Spotted");
                context.gameObject.GetComponent<enemyAnimController>().AlertServerRpc();
            }
            return true;
        }
        else
        {
            blackboard.Target = null;
            return false;
        }
    }
    private GameObject GetClosestEnemy(List<GameObject> enemies, Transform fromThis)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

}
