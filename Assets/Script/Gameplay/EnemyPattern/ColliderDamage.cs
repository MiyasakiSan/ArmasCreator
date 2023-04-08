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

    public bool beam;

    private GameplayController gameplayController;

    private void Start()
    {
        gameplayController = SharedContext.Instance.Get<GameplayController>();

        attackCollider.enabled = false;
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
        yield return new WaitForSeconds(ps.main.duration - 0.25f);
        isInit = false;
        attackCollider.enabled = false;
        yield return new WaitForSeconds(0.25f);
        attackCollider.enabled = false;
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

    public void DisableCollder()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        if (isHit) { return; }
        if (!isInit) { return; }

        var playerMovement = other.GetComponent<PlayerRpgMovement>();
        var Target = other.GetComponent<AttackTarget>();

        if (!playerMovement.isDodging)
        {
            isHit = true;
            attackCollider.enabled = false;

            if (beam)
            {
                StartCoroutine(beamHit());
            }

            Target.receiveAttack(damage * gameplayController.CurrentQuestInfo.InitATK);
            playerMovement.GetKnockback(this.transform.position);
        } 
    }

    IEnumerator beamHit()
    {
        yield return new WaitForSeconds(2f);
        isHit = false;
        attackCollider.enabled = true;
    }
}
