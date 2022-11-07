using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ArmasCreator.Gameplay
{
    public class OnTriggerEvent : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerStay;

        void OnTriggerStay(Collider col)
        {
            if (onTriggerStay != null) onTriggerStay.Invoke(col);
        }
    }
}

