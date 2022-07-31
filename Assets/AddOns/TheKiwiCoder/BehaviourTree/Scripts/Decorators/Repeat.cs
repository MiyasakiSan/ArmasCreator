using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder {
    public class Repeat : DecoratorNode {

        public bool restartOnSuccess = true;
        public bool restartOnFailure = false;
        public float DetectedRanged;
        protected override void OnStart() {

        }

        protected override void OnStop() {

        }

        protected override State OnUpdate() {
            switch (child.Update()) {
                case State.Running:
                    break;
                case State.Failure:
                    if (restartOnFailure) {
                        return State.Running;
                    } else {
                        return State.Failure;
                    }
                case State.Success:
                    if (restartOnSuccess)
                    {
                        return State.Running;
                    } else {
                        return State.Failure;
                    }
            }
            return State.Running;
        }
        public bool CheckIsPlayerNearBy()
        {
            List<GameObject> PlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
            GameObject nearestTarget = GetClosestEnemy(PlayerList, context.transform);
            if (Vector3.Distance(context.gameObject.transform.position, nearestTarget.transform.position) < DetectedRanged)
            {
                blackboard.Target = nearestTarget;
                Debug.Log("Enemy Spotted");
                return true;
            }
            else
            {
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

    
}
