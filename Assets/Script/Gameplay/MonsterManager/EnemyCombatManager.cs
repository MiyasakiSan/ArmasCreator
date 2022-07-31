using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class EnemyCombatManager : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;
    [SerializeField] enemyAnimController enemyAnim;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!enemyAnim.currentAnimatorStateBaseIsName("attack")) { return; }
        if (collision.gameObject.GetComponent<AttackTarget>() && !collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<AttackTarget>().receiveAttack(damage);
        }
    }
}
