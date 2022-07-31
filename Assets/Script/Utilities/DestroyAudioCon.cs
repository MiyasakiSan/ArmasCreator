using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudioCon : MonoBehaviour
{
    void Update()
    {
        DestroyImmediate(GameObject.Find("AudioSourceContinue").gameObject,false);
        Destroy(this.gameObject);
    }
}
