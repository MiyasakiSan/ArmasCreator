using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DestroyNetmanager : NetworkBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Destroy(NetworkManager.Singleton.gameObject);
        Destroy(this.gameObject);
    }
}
