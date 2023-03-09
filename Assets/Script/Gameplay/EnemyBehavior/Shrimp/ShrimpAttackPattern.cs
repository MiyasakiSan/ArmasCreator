using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using ArmasCreator.Utilities;

public class ShrimpAttackPattern : ActionNode
{
    private Coroutine attackCoroutine;
    private CoroutineHelper coroutineHelper;
    private enemyAnimController animController;
    private EnemyCombatManager enemyCombatManager;

    private Transform center;

    private Transform playerPos;

    private float playerSlope;

    public float radius;

    protected override void OnStart()
    {
        if (blackboard.Target != null)
        {
            playerPos = blackboard.Target.transform;
        }

        animController = context.gameObject.GetComponent<enemyAnimController>();
        enemyCombatManager = context.gameObject.GetComponent<EnemyCombatManager>();

        center = GameObject.Find("Center").GetComponent<Transform>();

        if (blackboard.CurrentAttackPattern != null)
        {
            Debug.Log($"ATTACK TARGET WITH {blackboard.CurrentAttackPattern.AttackAnimaiton.name}");

            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();

            if (blackboard.CurrentAttackPattern.IsFollow)
            {
                animController.SetAllLookAtWeight(1);
            }
            else
            {
                animController.ResetAllLookAt();
            }

            blackboard.IsAttacking = true;
            enemyCombatManager.IsAttacking = true;
            enemyCombatManager.currentAttackPattern = blackboard.CurrentAttackPattern;
            animController.isFollwPlayer = blackboard.CurrentAttackPattern.IsFollow;

            if (attackCoroutine == null)
            {
                attackCoroutine = coroutineHelper.Play(attackingCoroutine(blackboard.CurrentAttackPattern.AttackDuration));
            }
        }
        else
        {
            animController.ResetAllLookAt();
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {

        if (!blackboard.IsAttacking)
        {
            return State.Success;
        }
        else
        {
            if (animController.isFollwPlayer)
            {
                CalculateProjection();
            }

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

        context.transform.position = Vector3.Lerp(context.transform.position, new Vector3(xPos, context.transform.position.y, yPos), Time.deltaTime);

        var lookPos = playerPos.position - context.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, rotation, Time.deltaTime * 20);
    }

    private IEnumerator attackingCoroutine(float length)
    {
        animController.SetMoving(false);

        yield return new WaitForSeconds(0.25f);

        animController.SetAnimationRootNode(true);
        animController.RunAnimation(blackboard.CurrentAttackPattern.AttackAnimaiton);

        yield return new WaitForSeconds(length);

        blackboard.PrevioustAttackPattern = blackboard.CurrentAttackPattern;

        if (blackboard.CurrentAttackPattern.IsEnrageFinishMove)
        {
            blackboard.canUseEnrageFinishMove = false;
        }

        blackboard.IsAttacking = false;
        enemyCombatManager.IsAttacking = false;
        enemyCombatManager.currentAttackPattern = null;
        animController.SetAnimationRootNode(false);

        coroutineHelper.Stop(attackCoroutine);

        attackCoroutine = null;
    }
}
