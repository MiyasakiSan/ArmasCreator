using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace ArmasCreator.Behavior
{
    public class AttackPatternController : ActionNode
    {
        [SerializeField]
        private List<AttackPattern> allAttackPattern;

        [SerializeField]
        private int sameAttack;
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (setCurrentAttackPattern())
            {
                return State.Success;
            }
            else
            {
                return State.Failure;
            }
        }

        private bool setCurrentAttackPattern()
        {
            if(allAttackPattern.Count <= 0) { return false; }

            var currentAttackPattern = new AttackPattern();

            GetCurrentAttackPattern(out currentAttackPattern);

            blackboard.CurrentAttackPattern = currentAttackPattern;
            return true;
        }

        private void GetCurrentAttackPattern(out AttackPattern currentAttackPattern) 
        {
            currentAttackPattern = new AttackPattern();
            GetAvaliableAttackPattern(out List<AttackPattern> avaliableAttackPAttern);

            int randNum = Random.Range(0, avaliableAttackPAttern.Count - 1);

            currentAttackPattern = allAttackPattern[randNum];

        }

        private void GetAvaliableAttackPattern( out List<AttackPattern> avaliableAttackPattern)
        {
            avaliableAttackPattern = new List<AttackPattern>();
            var distance = Vector2.Distance(blackboard.Target.transform.position, context.gameObject.transform.position);

            foreach (var attackPatten in allAttackPattern)
            {
                if(attackPatten.ActiveCount == sameAttack)
                {
                    attackPatten.ResetActiveCount();
                    continue;
                }

                if(attackPatten.ActiveDistance > distance)
                {
                    continue;
                }

                if (!CheckDirection(attackPatten))
                {
                    continue;
                }

                avaliableAttackPattern.Add(attackPatten);
            }
        }

        private bool CheckDirection(AttackPattern attackPattern)
        {
            var playerDirection = Vector3.Normalize(context.gameObject.transform.position - blackboard.Target.transform.position);

            if (Vector3.Dot(playerDirection, attackPattern.ActiveDirection) > 0.75f)
            {
                return true;
            }

            return false;
        }
    }
}

