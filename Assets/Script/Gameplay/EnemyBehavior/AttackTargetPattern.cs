using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using ArmasCreator.Utilities;

public class AttackTargetPattern : ActionNode
{
    private Coroutine attackCoroutine;
    private CoroutineHelper coroutineHelper;
    private enemyAnimController animController;
    private EnemyCombatManager enemyCombatManager;

    protected override void OnStart() {

        animController = context.gameObject.GetComponent<enemyAnimController>();
        enemyCombatManager = context.gameObject.GetComponent<EnemyCombatManager>();

        if (blackboard.CurrentAttackPattern != null) 
        { 
            Debug.Log($"ATTACK TARGET WITH {blackboard.CurrentAttackPattern.AttackAnimaiton.name}");

            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();

            animController.SetAnimationRootNode(true);
            animController.RunAnimation(blackboard.CurrentAttackPattern.AttackAnimaiton);
            animController.ResetAllLookAt();

            blackboard.IsAttacking = true;
            enemyCombatManager.IsAttacking = true;
            enemyCombatManager.currentAttackPattern = blackboard.CurrentAttackPattern;
            context.transform.LookAt(blackboard.Target.transform);

            if (attackCoroutine == null)
            {
                attackCoroutine = coroutineHelper.Play(attackingCoroutine(blackboard.CurrentAttackPattern.AttackAnimaiton.length));
            }
        }

        animController.ResetAllLookAt();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (!blackboard.IsAttacking)
        {
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }

    private IEnumerator attackingCoroutine(float length)
    {
        yield return new WaitForSeconds(length);

        blackboard.IsAttacking = false;
        enemyCombatManager.IsAttacking = false;
        enemyCombatManager.currentAttackPattern = null;
        animController.SetAnimationRootNode(false);

        coroutineHelper.Stop(attackCoroutine);

        attackCoroutine = null;
    }
}
