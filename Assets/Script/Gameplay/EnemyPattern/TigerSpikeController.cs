using ArmasCreator.Utilities;
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

        public MonsterSFX monsterSFX;

        private GameplayController gameplayController;

        private void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
            gameplayController = SharedContext.Instance.Get<GameplayController>();
        }

        void Update()
        {
            if (isHittingGround) { return; }

            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Walkable"))
            {
                isHittingGround = true;
                rb.velocity = Vector3.zero;

                this.transform.parent = other.transform;

                isHit = true;

                monsterSFX.PlayTigerSpikeGroundFX();

                Destroy(this.gameObject, 4f);
            }

            if (isHit) { return; }

            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<AttackTarget>().receiveAttack(Damage);
                gameplayController.UpdatePlayerDamageTaken(Damage);

                Debug.Log("Hit player");

                Destroy(this.gameObject, 4f);

                isHit = true;
            }
        }
    }
}

