using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HideStartGame : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!NetworkManager.Singleton.IsHost)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            this.gameObject.SetActive(false);
        }
    }
}
