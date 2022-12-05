using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Netcode;

public class UseVFX : MonoBehaviour
{
    public VisualEffect[] VisualEffects;

    public ParticleSystem[] ParticleSystems;

    public void OnUseVFX(int VFXNumber)
    {
        VisualEffects[VFXNumber].Play();
    }


    public void OnPlayVFXDuration(int VFXNumber)
    {
        StartCoroutine(Duration(VFXNumber));
    }

    IEnumerator Duration(int VFXNumber)
    {
        VisualEffects[VFXNumber].Play();
        yield return new WaitForSeconds(2f);
        VisualEffects[VFXNumber].Stop();
    }

    public void OnUsePS(int VFXNumber)
    {
        ParticleSystem[] particleSystems = ParticleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
        ParticleSystems[VFXNumber].enableEmission = true;
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.enableEmission = true;
        }
        
    }

    public void OnStopPS(int VFXNumber)
    {
        ParticleSystem[] particleSystems = ParticleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
        ParticleSystems[VFXNumber].enableEmission = false;
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.enableEmission = false;
        }
    }
}
