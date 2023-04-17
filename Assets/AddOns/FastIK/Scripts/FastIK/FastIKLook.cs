﻿using System.Collections;
using UnityEngine;

namespace DitzelGames.FastIK
{
    public class FastIKLook : MonoBehaviour
    {
        /// <summary>
        /// Look at target
        /// </summary>
        public Transform Target;

        public Transform Parent;

        public float weight;

        private Coroutine resetWeightCoroutine;
        private Coroutine setWeightCoroutine;
        private Vector3 latestAngle;

        public bool isNPC;
        public float NPCrange;

        /// <summary>
        /// Initial direction
        /// </summary>
        protected Vector3 StartDirection;

        /// <summary>
        /// Initial Rotation
        /// </summary>
        protected Quaternion StartRotation;
        private Vector3 startEuler;

        void Awake()
        {
            if (Target == null)
                return;

            StartDirection = Target.position - transform.position;
            StartRotation = transform.rotation;
            startEuler = transform.eulerAngles;
        }

        private IEnumerator ResetWeightEnumurator()
        {
            while (weight > 0)
            {
                weight -= Time.deltaTime * 2;
                yield return null;
            }

            weight = 0;
            resetWeightCoroutine = null;
        }

        private IEnumerator SetWeightEnumurator(float amount)
        {
            while (weight < amount)
            {
                weight += Time.deltaTime * 2;
                yield return null;
            }

            weight = amount;
            setWeightCoroutine = null;
        }

        void LateUpdate()
        {
            if (Target == null)
            {
                return;
            }

            var dir = Vector3.Normalize(Target.position - Parent.position);

            var dot = Vector3.Dot(dir, Parent.forward);

            if(Vector3.Distance(Target.position,Parent.position) < 4 && !isNPC)
            {
                return;
            }

            if (Vector3.Distance(Target.position, Parent.position) > NPCrange && isNPC)
            {
                transform.eulerAngles = startEuler;
                return;
            }

            if (dot < Mathf.Cos(80) && isNPC)
            {
                transform.eulerAngles = startEuler;
                return;
            }

            if (dot < Mathf.Cos(80))
            {
                transform.eulerAngles = latestAngle;
                return;
            }

            if (weight == 0)
            {
                return;
            }
            else
            {
                var rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Parent.transform.forward, Target.position - transform.position) * transform.rotation, weight);

                var angle = rotation.eulerAngles;
                transform.eulerAngles = angle;
                latestAngle = angle;
            }
        }

        public void UpdateStartValue()
        {
            StartDirection = Target.position - transform.position;
            StartRotation = transform.rotation;
        }

        public void ResetWeight()
        {
            if (setWeightCoroutine != null)
            {
                StopCoroutine(setWeightCoroutine);
                setWeightCoroutine = null;
            }

            if(resetWeightCoroutine != null)
            {
                StopCoroutine(resetWeightCoroutine);
                resetWeightCoroutine = null;
            }

            UpdateStartValue();
            resetWeightCoroutine = StartCoroutine(ResetWeightEnumurator());
        }

        public void SetWeight(float amount)
        {
            if (resetWeightCoroutine != null)
            {
                StopCoroutine(resetWeightCoroutine);
                resetWeightCoroutine = null;
            }

            if(setWeightCoroutine != null)
            {
                StopCoroutine(setWeightCoroutine);
                setWeightCoroutine = null;
            }

            UpdateStartValue();
            setWeightCoroutine = StartCoroutine(SetWeightEnumurator(amount));
        }
    }
}
