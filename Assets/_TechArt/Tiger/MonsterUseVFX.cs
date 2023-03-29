using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Netcode;
using ArmasCreator.Gameplay;
using ArmasCreator.Utilities;

public class MonsterUseVFX : MonoBehaviour
{   
    [Header("NormalMode")]
    [SerializeField]
    private VisualEffect[] visualEffects;

    [SerializeField]
    private ParticleSystem[] particleSystems;

    [Header("RageMode")]
    [SerializeField]
    private VisualEffect[] visualEffectsRage;

    [SerializeField]
    private ParticleSystem[] particleSystemsRage;

    [SerializeField]
    private GameObject[] rageEffect;

    [SerializeField]
    private bool isRage;

    public bool IsRageStatus => isRage;

    [Header("Shrimp")]
    [SerializeField] private VisualEffect beamVFX;
    [SerializeField] private GameObject waterParticle;
    [Space(10)]
    [SerializeField] private GameObject BubblePrefab;
    [SerializeField] private List<GameObject> BubbleTargetPos;
    [SerializeField] private GameObject BubbleSpawnPos;
    [Space(10)]
    [SerializeField] private GameObject PlasmaBallPrefab;

    [SerializeField] private Material eyeMat;
    private Color eyeDefaultColor;

    private EnemyCombatManager enemyCombat;
    private GameplayController gameplayController;
    private MonsterSFX monsterSFX;

    private void Start()
    {
        if (eyeMat != null)
        {
            eyeDefaultColor = eyeMat.GetColor("_EmissionColor");
        }

        enemyCombat = GetComponent<EnemyCombatManager>();
        gameplayController = SharedContext.Instance.Get<GameplayController>();
        monsterSFX = GetComponent<MonsterSFX>();

        IsEnterRageMode(false);
    }

    public void OnUseVFX(int VFXNumber)
    {
        if(!isRage)
        {
            visualEffects[VFXNumber].Play();
        }
        else
        {
            visualEffectsRage[VFXNumber].Play();
        }
    }

    public void OnStopVFX(int VFXNumber)
    {
        if (!isRage)
        {
            visualEffects[VFXNumber].Stop();
        }
        else
        {
            visualEffectsRage[VFXNumber].Stop();
        }
    }

    public void OnPlayVFXDuration(int VFXNumber)
    {
        StartCoroutine(Duration(VFXNumber));
    }

    IEnumerator Duration(int VFXNumber)
    {
        if (!isRage)
        {
            visualEffects[VFXNumber].Play();
            //yield return new WaitForSeconds(visualEffects[VFXNumber].GetFloat(99));
            yield return new WaitForSeconds(1f);
            visualEffects[VFXNumber].Stop();
        }
        else
        {
            visualEffectsRage[VFXNumber].Play();
            //yield return new WaitForSeconds(visualEffects[VFXNumber].GetFloat(99));
            yield return new WaitForSeconds(1f);
            visualEffectsRage[VFXNumber].Stop();
        }
    }

    public void OnUsePS(int VFXNumber)
    {
        if (!isRage)
        {
            particleSystems[VFXNumber].gameObject.SetActive(true);
        }
        else
        {
            particleSystemsRage[VFXNumber].gameObject.SetActive(true);
        }
    }

    public void OnStopPS(int VFXNumber)
    {
        if (!isRage)
        {
            particleSystems[VFXNumber].gameObject.SetActive(false);
        }
        else
        {
            particleSystemsRage[VFXNumber].gameObject.SetActive(false);
        }
    }

    public void OnStopAllPS()
    {
        for (int VFXNumber = 0; VFXNumber < particleSystems.Length; VFXNumber++)
        {
            particleSystems[VFXNumber].gameObject.SetActive(false);
        }

        for (int VFXNumber = 0; VFXNumber < particleSystemsRage.Length; VFXNumber++)
        {
            particleSystemsRage[VFXNumber].gameObject.SetActive(false);
        }
    }

    public void IsRage(bool isRaged)
    {
        isRage = isRaged;
    }

    public void IsEnterRageMode(bool status)
    {
        IsRage(status);

        foreach(var effect in rageEffect)
        {
            effect.SetActive(status);
        }

        if (status)
        {
            if(gameplayController.EnemyType == ArmasCreator.GameData.SubType.Shrimp)
            {
                monsterSFX.PlayShrimpLightingSFX();
            }
            else
            {

            }
        }
        else
        {
            if (gameplayController.EnemyType == ArmasCreator.GameData.SubType.Shrimp)
            {
                monsterSFX.StopShrimpLightingSFX();
            }
            else
            {

            }
        }

        
    }

    public void SpawnBubble(int amount)
    {
        GameObject[] allBubble = null;
        allBubble = GameObject.FindGameObjectsWithTag("Bubble");

        foreach(var bubble in allBubble)
        {
            Destroy(bubble);
        }

        for(int i = 0; i< amount; i++)
        {
            var bubble = Instantiate(BubblePrefab, BubbleSpawnPos.transform.position, Quaternion.identity);
            bubble.GetComponent<Bubble>().Target = BubbleTargetPos[i];
            bubble.GetComponent<ColliderDamage>().SetupAttackPattern(enemyCombat.currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);
            bubble.GetComponent<ParticleSystem>().trigger.AddCollider(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
            bubble.GetComponent<ParticleSystem>().trigger.AddCollider(GetComponent<Collider>());
        }
    }

    public void SpawnPlasmaBall()
    {
        if (!isRage) { return; }

        var plasmaBall = Instantiate(PlasmaBallPrefab, BubbleSpawnPos.transform.position, Quaternion.identity);
        plasmaBall.GetComponent<FollowProjectile>().SetTarget(GameObject.FindGameObjectWithTag("Player"));
        plasmaBall.GetComponent<ColliderDamage>().SetupAttackPattern(enemyCombat.currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);

        monsterSFX.PlayPlasmaBall(plasmaBall,plasmaBall.GetComponent<Rigidbody>());

        Invoke("StopPlasmaSound", 10f);
        Destroy(plasmaBall, 10f);
    }

    void StopPlasmaSound()
    {
        monsterSFX.StopPlasmaBall();
    }

    public void OpenBeam(float lifeTime)
    {
        StartCoroutine(OpenBeamFor(lifeTime));
    }

    IEnumerator OpenBeamFor(float lifeTime)
    {
        beamVFX.SetFloat("Lifetime", lifeTime + 0.25f);
        beamVFX.Play();
        beamVFX.GetComponent<ColliderDamage>().SetupAttackPattern(enemyCombat.currentAttackPattern.Damage * gameplayController.CurrentQuestInfo.InitATK);
        waterParticle.SetActive(true);

        yield return new WaitForSeconds(lifeTime);

        waterParticle.SetActive(false);
    }

    public void EyeBlack()
    {
        eyeMat.SetColor("_EmissionColor", Color.black);
    }

    public void ResetEye()
    {
        eyeMat.SetColor("_EmissionColor", eyeDefaultColor);
    }

    public void EyeRed()
    {
        eyeMat.SetColor("_EmissionColor", Color.red);
    }
}
