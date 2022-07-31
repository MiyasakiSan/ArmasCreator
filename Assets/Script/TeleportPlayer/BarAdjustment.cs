using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BarAdjustment : MonoBehaviour
{
    public GameObject Player;
    // Update is called once per frame
    void Update()
    {
        if(Player == null)
        {
            DestroyServerRpc();
        }
    }
    [ServerRpc]
    public void DestroyServerRpc()
    {
        DestroyClientRpc();
    }
    [ClientRpc]
    public void DestroyClientRpc()
    {
        Destroy(gameObject);
    }
}
