using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Unity.Netcode;

public class UseVFX : MonoBehaviour
{
    public VisualEffect[] VisualEffects;

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
}
