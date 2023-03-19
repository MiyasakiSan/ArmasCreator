using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Netcode;

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

    private void Start()
    {
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
    }
}
