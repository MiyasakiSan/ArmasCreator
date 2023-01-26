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

        private void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (isHittingGround) { return; }

            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Walkable"))
            {
                isHittingGround = true;
                rb.velocity = Vector3.zero;

                this.transform.parent = collision.transform;
            }
        }
    }
}

