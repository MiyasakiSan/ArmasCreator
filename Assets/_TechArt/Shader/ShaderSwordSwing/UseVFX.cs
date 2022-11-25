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

    public void OnUseParticleSystem(int ParticleSystemNumber)
    {
        ParticleSystems[ParticleSystemNumber].enableEmission = true;
    }


    public void OnPlayParticleSystemDuration(int ParticleSystemNumber)
    {
        StartCoroutine(ParticleDuration(ParticleSystemNumber));
    }

    IEnumerator ParticleDuration(int ParticleNumber)
    {
        ParticleSystems[ParticleNumber].enableEmission = true;
        yield return new WaitForSeconds(2f);
        ParticleSystems[ParticleNumber].enableEmission = false;
    }
}
