using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using ArmasCreator.Behavior;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;
using TheKiwiCoder;
using ArmasCreator.Gameplay.UI;
using UnityEditor;

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
    private List<EnemyBoxCollider> hitBoxColliderList;

    [SerializeField]
    private List<EnemyBoxCollider> hurtBoxColliderList;

    public List<AttackPattern> AllAttackPattern;

    [SerializeField]
    private BehaviourTreeRunner BHT;

    private GameplayController gameplayController;

    private UIPlayerController uIPlayerController;

    public bool IsAttacking;

    public AttackPattern currentAttackPattern;

    private Coroutine stunCoroutine;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void Init()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();

        uIPlayerController = SharedContext.Instance.Get<UIPlayerController>();

        if (hitBoxColliderList.Count <= 0) { return; }

        foreach(EnemyBoxCollider enemyBoxCollider in hitBoxColliderList)
        {
            enemyBoxCollider.onTriggerEnter.AddListener(OnHitBoxTriggerEnter);
        }

        foreach (EnemyBoxCollider enemyBoxCollider in hurtBoxColliderList)
        {
            enemyBoxCollider.onTriggerEnter.AddListener(OnHurtBoxTriggerEnter);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var attackPattern in AllAttackPattern)
        {
            if(attackPattern.ActiveDistance >= 15)
            {
                Gizmos.color = Color.red;
            }
            else if(attackPattern.ActiveDistance >= 8)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            Handles.Label(transform.position + Quaternion.Euler(0, transform.eulerAngles.y, 0) * attackPattern.ActiveDirection * attackPattern.ActiveDistance/2 + Vector3.up * 5, attackPattern.name);

            var pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            GizmosExtensions.DrawWireArc(pos, Quaternion.Euler(0, transform.eulerAngles.y, 0) * attackPattern.ActiveDirection, attackPattern.ActiveAngleOffset, attackPattern.ActiveDistance);
            GizmosExtensions.DrawWireArc(pos, Quaternion.Euler(0, transform.eulerAngles.y, 0) * attackPattern.ActiveDirection, attackPattern.ActiveAngleOffset, attackPattern.MinActiveDistance);
        }
    }

    private void OnHitBoxTriggerEnter(Collider col,GameObject gameObject)
    {
        if (!col.CompareTag("Weapon")) { return;  }

        Debug.Log($"กูเจ็บ ตรง {gameObject} เพราะ โดน {col.gameObject.name}");

        float damage = col.GetComponent<Weapon>().weaponDamage;

        var playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        enemyStat.receiveAttack(damage + ((playerStat.Atk * damage) /100));

        //var contactPoint = gameObject.GetComponent<CapsuleCollider>().ClosestPoint(col.gameObject.transform.position);

        Vector3 contactPoint;

        //contactPoint = gameObject.GetComponent<CapsuleCollider>().ClosestPoint(col.gameObject.transform.position);

        contactPoint = gameObject.transform.position;

        uIPlayerController.ShowDamage(contactPoint, damage);

        gameplayController.UpdatePlayerDamageDelt(damage);
    }

    private void OnHurtBoxTriggerEnter(Collider col, GameObject gameObject)
    {
        if (!col.CompareTag("Player")) { return; }

        if (!IsAttacking) { return; }

        if (currentAttackPattern == null) { return; }

        if (currentAttackPattern.HurtBoxName != gameObject.name) { return; }

        Debug.Log($"{col.gameObject.name} โดนตี เพราะ โดน {gameObject}");

        col.gameObject.GetComponent<AttackTarget>().receiveAttack(currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);
        gameplayController.UpdatePlayerDamageTaken(currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);

        if (currentAttackPattern.IsKnockback)
        {
            col.gameObject.GetComponent<PlayerRpgMovement>().GetKnockback(transform.position);
        }
        else
        {
            col.gameObject.GetComponent<PlayerRpgMovement>().GetHit();
        }

        uIPlayerController.ShowHurt();
    }

    public void Stun()
    {
        if (stunCoroutine != null) { return; }

        stunCoroutine = StartCoroutine(EnemyStun());
    }

    IEnumerator EnemyStun()
    {
        StopBTH();
        enemyAnim.anim.Play("idleToStun");
        enemyAnim.anim.SetBool("isStun", true);
        enemyAnim.SetAnimationRootNode(true);

        yield return new WaitForSeconds(5f);

        enemyAnim.anim.SetBool("isStun", false);
        yield return new WaitForSeconds(0.5f);
        enemyAnim.anim.Play("jumpBack");
        yield return new WaitForSeconds(2f);

        StartBTH();
        enemyAnim.SetAnimationRootNode(false);
        stunCoroutine = null;
    }

    public void StopBTH()
    {
        BHT.enabled = false;
    }

    public void StartBTH()
    {
        BHT.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bubble")) { return; }

        Stun();
    }

    private void OnDestroy()
    {
        if (hitBoxColliderList.Count <= 0) { return; }

        foreach (EnemyBoxCollider enemyBoxCollider in hitBoxColliderList)
        {
            enemyBoxCollider.onTriggerEnter.RemoveListener(OnHitBoxTriggerEnter);
        }
    }
}
