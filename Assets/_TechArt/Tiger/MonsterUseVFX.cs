using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Netcode;

public class MonsterUseVFX : MonoBehaviour
{
    [SerializeField]
    private VisualEffect[] visualEffects;

    [SerializeField]
    private ParticleSystem[] particleSystems;



    public void OnUseVFX(int VFXNumber)
    {
        visualEffects[VFXNumber].Play();
    }


    public void OnPlayVFXDuration(int VFXNumber)
    {
        StartCoroutine(Duration(VFXNumber));
    }

    IEnumerator Duration(int VFXNumber)
    {
        visualEffects[VFXNumber].Play();
        yield return new WaitForSeconds(visualEffects[VFXNumber].GetFloat(99));
        visualEffects[VFXNumber].Stop();
    }

    public void OnUsePS(int VFXNumber)
    {
        ParticleSystem[] particleSystemArray = particleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystemArray)
        {
            particleSystem.gameObject.SetActive(true);
        }
    }

    public void OnStopPS(int VFXNumber)
    {
        ParticleSystem[] particleSystemArray = particleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in particleSystemArray)
        {
            particleSystem.gameObject.SetActive(false);
        }
    }

    public void OnStopAllPS()
    {
        for (int VFXNumber = 0; VFXNumber < particleSystems.Length; VFXNumber++)
        {
            ParticleSystem[] particleSystemArray = particleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particleSystem in particleSystemArray)
            {
                particleSystem.gameObject.SetActive(false);
            }
        }
    }
}
