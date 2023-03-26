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

    [SerializeField]
    private List<ParticleSystem> particleSystemList;

    [SerializeField]
    private bool isLoop;

    private float damage;
    private bool isHit;
    private bool isInit;

    private GameplayController gameplayController;

    private void Start()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();
    }

    public void RunVFX()
    {
        Debug.Log("Show VFX");

        if (VFXList.Count == 0) { return; }

        if (particleSystemList.Count == 0) { return; }

        foreach (var VFX in VFXList)
        {
            VFX.Play();
        }

        foreach(var ps in particleSystemList)
        {
            ps.gameObject.SetActive(true);
            if (isLoop == false)
            {
                StartCoroutine(waitUntilEffectFinish(ps));
            }
        }
    }

    public IEnumerator waitUntilEffectFinish(ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration);
        ps.gameObject.SetActive(false);
    }

    public void SetupAttackPattern(float damage)
    {
        this.damage = damage;
        isHit = false;
        attackCollider.enabled = true;

        if (anim != null)
        {
            anim.SetBool("init", true);
            RunVFX();
        }

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
