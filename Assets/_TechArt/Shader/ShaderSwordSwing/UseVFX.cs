using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Netcode;

public class UseVFX : MonoBehaviour
{
    [SerializeField]
    private CombatRpgManager combatManager;

    [SerializeField]
    private VisualEffect[] visualEffects;

    [SerializeField]
    private VisualEffect[] visualEffectsMagMode;

    [SerializeField]
    private ParticleSystem[] particleSystems;

    [SerializeField]
    private ParticleSystem[] particleSystemsMagMode;

    [SerializeField]
    private bool isMagnetMode => combatManager.IsEMTState;

    public void OnUseVFX(int VFXNumber)
    {
        if (isMagnetMode)
        {
            visualEffectsMagMode[VFXNumber].Play();
        }
        else
        {
            visualEffects[VFXNumber].Play();
        }
    }


    public void OnPlayVFXDuration(int VFXNumber)
    {
        if(isMagnetMode)
        {
            StartCoroutine(DurationMagnet(VFXNumber));
        }
        else
        {
            StartCoroutine(Duration(VFXNumber));
        }
    }

    IEnumerator Duration(int VFXNumber)
    {
        visualEffects[VFXNumber].Play();
        yield return new WaitForSeconds(2f);
        visualEffects[VFXNumber].Stop();
    }

    IEnumerator DurationMagnet(int VFXNumber)
    {
        visualEffectsMagMode[VFXNumber].Play();
        yield return new WaitForSeconds(2f);
        visualEffectsMagMode[VFXNumber].Stop();
    }

    public void OnUsePS(int VFXNumber)
    {
        if(isMagnetMode)
        {
            ParticleSystem[] particleSystemArray = particleSystemsMagMode[VFXNumber].GetComponentsInChildren<ParticleSystem>();
            particleSystemsMagMode[VFXNumber].enableEmission = true;
            foreach (ParticleSystem particleSystem in particleSystemArray)
            {
                particleSystem.enableEmission = true;
            }
        }
        else
        {
            ParticleSystem[] particleSystemArray = particleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
            particleSystems[VFXNumber].enableEmission = true;
            foreach (ParticleSystem particleSystem in particleSystemArray)
            {
                particleSystem.enableEmission = true;
            }
        }
    }

    public void OnStopPS(int VFXNumber)
    {

        ParticleSystem[] particleSystemArray = particleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
        particleSystems[VFXNumber].enableEmission = false;
        foreach (ParticleSystem particleSystem in particleSystemArray)
        {
            particleSystem.enableEmission = false;
        }
        particleSystemArray = particleSystemsMagMode[VFXNumber].GetComponentsInChildren<ParticleSystem>();
        particleSystemsMagMode[VFXNumber].enableEmission = false;
        foreach (ParticleSystem particleSystem in particleSystemArray)
        {
            particleSystem.enableEmission = false;
        }
    }

    public void OnStopAllPS()
    {
        for (int VFXNumber = 0; VFXNumber < particleSystems.Length; VFXNumber++)
        {
            ParticleSystem[] particleSystemArray = particleSystems[VFXNumber].GetComponentsInChildren<ParticleSystem>();
            particleSystems[VFXNumber].enableEmission = false;
            foreach (ParticleSystem particleSystem in particleSystemArray)
            {
                particleSystem.enableEmission = false;
            }
        }

        for (int VFXNumber = 0; VFXNumber < particleSystemsMagMode.Length; VFXNumber++)
        {
            ParticleSystem[] particleSystemArray = particleSystemsMagMode[VFXNumber].GetComponentsInChildren<ParticleSystem>();
            particleSystemsMagMode[VFXNumber].enableEmission = false;
            foreach (ParticleSystem particleSystem in particleSystemArray)
            {
                particleSystem.enableEmission = false;
            }
        }
    }

    //public void SetIsMagnetMode(bool isMagnetMode)
    //{
    //    this.isMagnetMode = isMagnetMode;
    //}
}
