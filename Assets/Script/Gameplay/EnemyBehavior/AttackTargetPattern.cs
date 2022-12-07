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

    protected override void OnStart() {

        animController = context.gameObject.GetComponent<enemyAnimController>();

        if (blackboard.CurrentAttackPattern != null) 
        { 
            Debug.Log($"ATTACK TARGET WITH {blackboard.CurrentAttackPattern.AttackAnimaiton.name}");

            coroutineHelper = SharedContext.Instance.Get<CoroutineHelper>();

            animController.SetAnimationRootNode(true);
            animController.RunAnimation(blackboard.CurrentAttackPattern.AttackAnimaiton);
            animController.ResetAllLookAt();

            blackboard.IsAttacking = true;

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
        animController.SetAnimationRootNode(false);
        animController.SetAllLookAtWeight(1);

        coroutineHelper.Stop(attackCoroutine);

        attackCoroutine = null;
    }
}
