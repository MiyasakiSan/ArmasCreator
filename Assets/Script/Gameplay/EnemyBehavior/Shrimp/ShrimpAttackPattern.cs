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
                var lookPos = blackboard.Target.transform.position - context.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);

                context.transform.rotation = Quaternion.Slerp(context.transform.rotation, rotation, Time.deltaTime * 20);
            }

            return State.Running;
        }
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
