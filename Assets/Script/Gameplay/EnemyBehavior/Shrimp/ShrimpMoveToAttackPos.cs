using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ShrimpMoveToAttackPos : ActionNode
{
    public bool disable;

    private Transform center;

    private Transform playerPos;

    private float playerSlope;

    public float radius;

    private enemyAnimController animController;

    protected override void OnStart() {

        if (blackboard.Target != null)
        {
            playerPos = blackboard.Target.transform;
        }

        center = GameObject.Find("Center").GetComponent<Transform>();

        animController = context.gameObject.GetComponent<enemyAnimController>();

        animController.ResetAllLookAt();

    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() 
    {
        if (disable) { return State.Success; }

        if (blackboard.CurrentAttackPattern != null && !blackboard.IsRunToAttackPos)
        {
            if (blackboard.CurrentAttackPattern.AttackDistance == 0)
            {
                blackboard.IsRunToAttackPos = false;

                Debug.Log("Dont have attack distance");

                return State.Success;
            }

            blackboard.IsRunToAttackPos = true;
            return State.Running;
        }

        if (blackboard.IsRunToAttackPos)
        {
            CalculateProjection();

            if (Vector3.Distance(context.transform.position,blackboard.Target.transform.position) <= blackboard.CurrentAttackPattern.AttackDistance)
            {
                blackboard.IsRunToAttackPos = false;
                return State.Success;
            }

            return State.Running;
        }

        if (blackboard.CurrentAttackPattern == null)
        {
            Debug.Log(blackboard.CurrentAttackPattern);

            animController.SetMoving(false);
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }

    private void CalculateProjection()
    {
        if (playerPos.position.z != center.position.z)
        {
            playerSlope = (playerPos.position.x - center.position.x) / (playerPos.position.z - center.position.z);
        }
        else
        {
            playerSlope = 0;
        }


        float c = playerPos.position.z - (playerSlope * playerPos.position.x);

        float A, B, C;

        float x1 = 0, x2 = 0, disc, deno, xPos, yPos;

        A = Mathf.Pow(playerSlope, 2) + 1;
        B = 2 * playerSlope * c;
        C = Mathf.Pow(c, 2) - Mathf.Pow(radius, 2);

        if (A == 0)
        {
            x1 = -C / B;
        }
        else
        {
            disc = (B * B) - (4 * A * C);
            deno = 2 * A;
            if (disc > 0)
            {
                x1 = (-B / deno) + (Mathf.Sqrt(disc) / deno);
                x2 = (-B / deno) - (Mathf.Sqrt(disc) / deno);
            }
            else if (disc == 0)
            {
                x1 = -B / deno;
            }
            else
            {
                x1 = -B / deno;
                x2 = ((Mathf.Sqrt((4 * A * C) - (B * B))) / deno);
            }
        }

        if (playerPos.position.x >= 0)
        {
            xPos = x1;
        }
        else
        {
            xPos = x2;
        }

        yPos = playerSlope * xPos + c;

        Vector3 lerpVector = Vector3.Lerp(context.transform.position, new Vector3(xPos, context.transform.position.y, yPos), Time.deltaTime);

        Vector3 offsetVector = (playerPos.position - center.transform.position).normalized * 20;

        context.transform.position = lerpVector + new Vector3(offsetVector.x, 0, offsetVector.z); ;

        var lookPos = playerPos.position - context.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, rotation, Time.deltaTime * 10);
    }
}
