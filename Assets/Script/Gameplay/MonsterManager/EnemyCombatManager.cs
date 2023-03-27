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

    [SerializeField]
    private WaterGroundController waterGroundController;

    private GameplayController gameplayController;

    private UIPlayerController uIPlayerController;

    public bool IsAttacking;

    public AttackPattern currentAttackPattern;

    private Coroutine stunCoroutine;
    private bool isStun;
    private bool isUltimate;

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

        var playerMovement = col.gameObject.GetComponent<PlayerRpgMovement>();

        if (playerMovement.isDodging) { return; }

        Debug.Log($"{col.gameObject.name} โดนตี เพราะ โดน {gameObject}");

        col.gameObject.GetComponent<AttackTarget>().receiveAttack(currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);
        gameplayController.UpdatePlayerDamageTaken(currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);

        if (currentAttackPattern.IsKnockback)
        {
            playerMovement.GetKnockback(transform.position);
        }
        else
        {
            playerMovement.GetHit();
        }
    }

    public void GroundWater()
    {
        if (isUltimate) { return; }

        waterGroundController.InitGroundWater(currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);
        waterGroundController.onGroundWaterFinishedCallback += GroundWaterFinish;

        enemyAnim.anim.SetBool("isGroundWater", true);
    }

    void GroundWaterFinish()
    {
        enemyAnim.anim.SetBool("isGroundWater", false);
        waterGroundController.onGroundWaterFinishedCallback -= GroundWaterFinish;
    }

    void GroundThunderFinish()
    {
        isUltimate = false;
        Stun();
        waterGroundController.onGroundWaterFinishedCallback -= GroundThunderFinish;
    }

    public void GroundThunder()
    {
        if (!GetComponent<MonsterUseVFX>().IsRageStatus) { return; }

        if (isUltimate) { return; }

        waterGroundController.InitGroundThunder(currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);
    }

    public void GroundThunderUltimate()
    {
        StartCoroutine(InitGroundThunderUltimate());
    }

    IEnumerator InitGroundThunderUltimate()
    {
        var center = GameObject.Find("Center");
        enemyAnim.anim.enabled = false;
        isUltimate = true;

        while(Vector3.Distance(transform.position,center.transform.position) > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, center.transform.position, Time.deltaTime);
            yield return null;
        }

        enemyAnim.anim.enabled = true;
        enemyAnim.anim.Play("ultimateToGround");

        waterGroundController.InitUltimateGround(currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);
        waterGroundController.onGroundWaterFinishedCallback += GroundThunderFinish;
    }

    public void Stun()
    {
        if (stunCoroutine != null) { return; }

        if (isStun) { return; }

        stunCoroutine = StartCoroutine(EnemyStun());
    }

    IEnumerator EnemyStun()
    {
        StopBTH();
        isStun = true;
        enemyAnim.anim.Play("idleToStun");
        enemyAnim.anim.SetBool("isStun", true);
        enemyAnim.SetAnimationRootNode(true);

        yield return new WaitForSeconds(5f);

        enemyAnim.anim.SetBool("isStun", false);
        yield return new WaitForSeconds(0.5f);
        enemyAnim.anim.Play("jumpBack");
        yield return new WaitForSeconds(3f);

        StartBTH();
        enemyAnim.SetAnimationRootNode(false);
        stunCoroutine = null;
        isStun = false;
    }

    public void StopBTH()
    {
        BHT.enabled = false;
    }

    public void StartBTH()
    {
        BHT.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (IsAttacking) { return; }

        if (!other.gameObject.CompareTag("Bubble")) { return; }

        if (!other.gameObject.GetComponent<Bubble>().reached) { return; }

        if (isStun) { return; }

        GameObject[] allBubble = null;
        allBubble = GameObject.FindGameObjectsWithTag("Bubble");

        foreach (var bubble in allBubble)
        {
            Destroy(bubble);
        }

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
