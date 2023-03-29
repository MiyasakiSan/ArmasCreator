using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderFillAnimate : MonoBehaviour
{
    public Material ThunderFill;
    public AnimationCurve curve;
    private float value;
    public float Speed = 5;
    void Update()
    {
        value += 0.1f;
        if(value > 1)
        {
            value = 0;
        }
        ThunderFill.SetFloat("_LightningTexture", value);
        ThunderFill.SetVector("_Offset", new Vector2(value * Speed, 0));
    }
}
