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

    IEnumerator ParticleDuration(int VFXNumber)
    {
        var em = ParticleSystems[VFXNumber].emission;
        em.enabled = true;
        yield return new WaitForSeconds(0.5f);
        em.enabled = false;
    }

    public void OnSimulateParticle(int ParticleNumber)
    {
        StartCoroutine(ParticleDuration(ParticleNumber));
    }
}
