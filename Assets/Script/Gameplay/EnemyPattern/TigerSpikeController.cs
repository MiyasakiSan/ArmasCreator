using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmasCreator.Gameplay
{
    public class TigerSpikeController : MonoBehaviour
    {
        private float speed = 50f;

        private bool isHittingGround;

        private Rigidbody rb;

        public float Damage;

        private bool isHit;

        private void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (isHittingGround) { return; }

            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isHit) { return; }

            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<AttackTarget>().receiveAttack(Damage);

                isHit = true;
            }

            if (other.gameObject.CompareTag("Walkable"))
            {
                isHittingGround = true;
                rb.velocity = Vector3.zero;

                this.transform.parent = other.transform;

                isHit = true;

                StartCoroutine(autoDestroy());
            }
        }
        IEnumerator autoDestroy()
        {
            yield return new WaitForSeconds(4f);

            Destroy(this.gameObject);
        }
    }
}

