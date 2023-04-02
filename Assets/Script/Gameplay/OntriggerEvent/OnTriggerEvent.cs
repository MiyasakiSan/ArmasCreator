using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ArmasCreator.Gameplay
{
    public class OnTriggerEvent : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerStay;

        public UnityEvent<Collider> onTriggerEnter;

        public delegate void onPlayerDie();
        public onPlayerDie OnPlayerDieCallback;

        [SerializeField]
        private GameObject boxCollider;

        void OnTriggerStay(Collider col)
        {
            if (onTriggerStay != null) onTriggerStay.Invoke(col);
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.GetComponent<PlayerRpgMovement>().isDead) { return; }

            if (onTriggerEnter != null) onTriggerEnter.Invoke(col);
        }

        public void SetBoxCollider(bool active)
        {
            boxCollider.SetActive(active);
        }

        public void playerDieInvoke()
        {
            OnPlayerDieCallback?.Invoke();
            boxCollider.SetActive(false);
            Debug.Log("invoke die");
        }
    }
}

