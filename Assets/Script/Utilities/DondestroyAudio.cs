using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DondestroyAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject.GetComponent<DondestroyAudio>());
    }
    private void Update()
    {
        if(GameObject.FindGameObjectsWithTag("AudioCon").Length > 1){
            DestroyImmediate(GameObject.FindGameObjectsWithTag("AudioCon")[1], false);
        }
    }
}
