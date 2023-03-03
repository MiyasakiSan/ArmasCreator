﻿using System.Collections;
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

    private GameplayController gameplayController;

    private UIPlayerController uIPlayerController;

    public bool IsAttacking;

    public AttackPattern currentAttackPattern;

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

        enemyStat.receiveAttack(damage);

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

        col.gameObject.GetComponent<AttackTarget>().receiveAttack(currentAttackPattern.Damage);

        if (currentAttackPattern.IsKnockback)
        {
            col.gameObject.GetComponent<PlayerRpgMovement>().GetKnockback();
        }
        else
        {
            col.gameObject.GetComponent<PlayerRpgMovement>().GetHit();
        }

        uIPlayerController.ShowHurt();
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
