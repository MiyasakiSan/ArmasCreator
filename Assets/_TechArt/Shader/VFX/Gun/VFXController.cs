using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    [SerializeField]
    private List<VisualEffect> listVisualEffect = new List<VisualEffect>();
    public void OnPlayVFX(int index)
    {
        listVisualEffect[index].Play();
    }
    public void OnStopVFX(int index)
    {
        listVisualEffect[index].Stop();
    }
}
