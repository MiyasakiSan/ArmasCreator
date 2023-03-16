using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ColliderDamage : MonoBehaviour
{
    [SerializeField]
    private Collider attackCollider;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private List<VisualEffect> VFXList;

    private float damage;
    private bool isHit;
    private bool isInit;

    private GameplayController gameplayController;

    private void Start()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();
    }

    private void RunVFX()
    {
        Debug.Log("Show VFX");

        if (VFXList.Count == 0) { return; }

        foreach(var VFX in VFXList)
        {
            VFX.Play();
        }
    }

    public void SetupAttackPattern(float damage)
    {
        this.damage = damage;
        anim.SetBool("init",true);
        RunVFX();
        isInit = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        if (isHit) { return; }
        if (!isInit) { return; }

        isHit = true;
        attackCollider.enabled = false;

        var playerMovement = other.GetComponent<PlayerRpgMovement>();
        var Target = other.GetComponent<AttackTarget>();

        Target.receiveAttack(damage * gameplayController.CurrentQuestInfo.InitATK);
        playerMovement.GetKnockback(this.transform.position);
    }
}
