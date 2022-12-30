using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.Behavior;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;

public class EnemyCombatManager : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    private float damage;

    [SerializeField] 
    enemyAnimController enemyAnim;

    [SerializeField]
    private EnemyStat enemyStat;

    [SerializeField]
    private List<EnemyBoxCollider> enemyBoxColliderList;

    public List<AttackPattern> AllAttackPattern;

    private GameplayController gameplayController;
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
        gameplayController = SharedContext.Instance.Get<GameplayController>();

        if (enemyBoxColliderList.Count <= 0) { return; }

        foreach(EnemyBoxCollider enemyBoxCollider in enemyBoxColliderList)
        {
            enemyBoxCollider.onTriggerEnter.AddListener(OnHurtBoxTriggerEnter);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var attackPattern in AllAttackPattern)
        {
            if(attackPattern.ActiveDistance >= 30)
            {
                Gizmos.color = Color.red;
            }
            else if(attackPattern.ActiveDistance >= 15)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            var pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            GizmosExtensions.DrawWireArc(pos, Quaternion.Euler(0, transform.eulerAngles.y, 0) * attackPattern.ActiveDirection, attackPattern.ActiveAngleOffset, attackPattern.ActiveDistance);
        }
    }

    private void OnHurtBoxTriggerEnter(Collider col,GameObject gameObject)
    {
        if (!col.CompareTag("Weapon")) { return;  }

        Debug.Log($"กูเจ็บ ตรง {gameObject} เพราะ โดน {col.gameObject.name}");

        float damage = col.GetComponent<Weapon>().weaponDamage;

        enemyStat.receiveAttack(damage);

        gameplayController.UpdatePlayerDamageDelt(damage);
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
