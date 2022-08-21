using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnTeleport : MonoBehaviour
{
    public GameObject PrefabTeleport;
    public Vector3 SpawnPrefabAt;
    public Vector3 SpawnPlayerAt;
    public string TeleportPath;
    GameObject Teleport;
    GameObject TeleportClient;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Teleport != null) return;
        if (NetworkManager.Singleton.IsServer)
        {
            Teleport = Instantiate(PrefabTeleport, SpawnPrefabAt, Quaternion.identity);
            Teleport.GetComponent<TeleportParty>().SpawnAt = SpawnPlayerAt;
            Teleport.GetComponent<TeleportParty>().TeleportPath = TeleportPath;
            Teleport.GetComponent<NetworkObject>().Spawn();
        }
        if (TeleportClient != null) return;
        if (!NetworkManager.Singleton.IsServer)
        {
            TeleportClient = GameObject.Find("Teleport(Clone)");
            TeleportClient.GetComponent<TeleportParty>().SpawnAt = SpawnPlayerAt;
            TeleportClient.GetComponent<TeleportParty>().TeleportPath = TeleportPath;
        }
    }
}
