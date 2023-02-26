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

    protected override void OnStart()
    {

        animController = context.gameObject.GetComponent<enemyAnimController>();
        enemyCombatManager = context.gameObject.GetComponent<EnemyCombatManager>();

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
            context.transform.LookAt(blackboard.Target.transform);

            if (attackCoroutine == null)
            {
                attackCoroutine = coroutineHelper.Play(attackingCoroutine(blackboard.CurrentAttackPattern.AttackAnimaiton.length));
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
            if (blackboard.CurrentAttackPattern.IsFollow)
            {
                context.transform.LookAt(blackboard.Target.transform);
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

        blackboard.IsAttacking = false;
        enemyCombatManager.IsAttacking = false;
        enemyCombatManager.currentAttackPattern = null;
        animController.SetAnimationRootNode(false);

        coroutineHelper.Stop(attackCoroutine);

        attackCoroutine = null;
    }
}
