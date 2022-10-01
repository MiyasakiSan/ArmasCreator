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

        private List<AttackPatternContainer> allAttackPatternContainer = new List<AttackPatternContainer>();

        private bool isInit;

        protected override void OnStart()
        {
            GenerateAttackPatternContainer();
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

        private void GenerateAttackPatternContainer()
        {
            if(isInit) { return; }

            allAttackPattern = context.gameObject.GetComponent<EnemyCombatManager>().AllAttackPattern;

            foreach(var attackPattern in allAttackPattern)
            {
                allAttackPatternContainer.Add(new AttackPatternContainer(attackPattern,0));
            }

            // FOR TEST
            blackboard.Target = GameObject.FindObjectOfType<PlayerStat>().gameObject;

            isInit = true;
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

            int randNum = 0;

            if (avaliableAttackPAttern.Count > 0)
            {
                randNum = Random.Range(0, avaliableAttackPAttern.Count - 1);
                currentAttackPattern = avaliableAttackPAttern[randNum];

                AttackPattern attackPattern = currentAttackPattern;
                allAttackPatternContainer.Find(x => x.AttackPattern == attackPattern).Use();
            }
            else
            {
                currentAttackPattern = null;
            }
        }

        private void GetAvaliableAttackPattern( out List<AttackPattern> avaliableAttackPattern)
        {
            avaliableAttackPattern = new List<AttackPattern>();
            var distance = Vector2.Distance(blackboard.Target.transform.position, context.gameObject.transform.position);

            foreach (var attackPatternContainer in allAttackPatternContainer)
            {
                if(attackPatternContainer.UseCount == attackPatternContainer.AttackPattern.MaxActiveCount)
                {
                    attackPatternContainer.ResetUseCount();
                    continue;
                }

                if (distance > attackPatternContainer.AttackPattern.ActiveDistance )
                {
                    continue;
                }

                if (!CheckDirection(attackPatternContainer.AttackPattern))
                {
                    continue;
                }

                avaliableAttackPattern.Add(attackPatternContainer.AttackPattern);
            }
        }

        private bool CheckDirection(AttackPattern attackPattern)
        {
            var playerDirection = Vector3.Normalize(blackboard.Target.transform.position - context.gameObject.transform.position );
            var attackDirection = Vector3.Normalize(Quaternion.Euler(0, context.transform.eulerAngles.y, 0) * attackPattern.ActiveDirection);
            var directDot = Vector3.Dot(playerDirection, attackDirection);

            //Debug.LogError($"directDot : {directDot}   ||  offset :  { Mathf.Cos(attackPattern.ActiveAngleOffset / 2 * Mathf.Deg2Rad)}");

            if (directDot >= Mathf.Cos(attackPattern.ActiveAngleOffset/2 * Mathf.Deg2Rad))
            {
                return true;
            }

            return false;
        }
    }

    public struct AttackPatternContainer
    {
        private AttackPattern attackPattern;
        public AttackPattern AttackPattern => attackPattern;

        private int useCount;
        public int UseCount => useCount;

        public AttackPatternContainer(AttackPattern attackPattern,int useCount)
        {
            this.attackPattern = attackPattern;
            this.useCount = useCount;
        }

        public void ResetUseCount()
        {
            useCount = 0;
        }

        public void Use()
        {
            useCount++;
        }
    }
}

