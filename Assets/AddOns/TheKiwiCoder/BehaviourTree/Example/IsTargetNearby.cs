using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsTargetNearby : ActionNode
{
    public float DetectedRanged;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (!CheckIsPlayerNearBy())
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
    public bool CheckIsPlayerNearBy()
    {
        List<GameObject> PlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        GameObject nearestTarget = GetClosestEnemy(PlayerList, context.transform);
        if (Vector3.Distance(context.gameObject.transform.position, nearestTarget.transform.position) < DetectedRanged)
        {
            if(blackboard.Target != nearestTarget)
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
