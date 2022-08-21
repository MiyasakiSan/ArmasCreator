using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class EnemyCombatManager : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float damage;
    [SerializeField] enemyAnimController enemyAnim;
    [SerializeField]
    private List<EnemyBoxCollider> enemyBoxColliderList;

    void Start()
    {
        Init();
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
    
    private void Init()
    {
        if(enemyBoxColliderList.Count <= 0) { return; }

        foreach(EnemyBoxCollider enemyBoxCollider in enemyBoxColliderList)
        {
            enemyBoxCollider.onTriggerEnter.AddListener(OnHurtBoxTriggerEnter);
        }
    }

    private void OnHurtBoxTriggerEnter(Collider col,GameObject gameObject)
    {
        if (!col.CompareTag("Weapon")) { return;  }

        Debug.LogError($"กูเจ็บ ตรง {gameObject} เพราะ โดน {col.gameObject.name}");
    }

    private void OnDestroy()
    {
        if (enemyBoxColliderList.Count <= 0) { return; }

        foreach (EnemyBoxCollider enemyBoxCollider in enemyBoxColliderList)
        {
            enemyBoxCollider.onTriggerEnter.RemoveListener(OnHurtBoxTriggerEnter);
        }
    }
}
