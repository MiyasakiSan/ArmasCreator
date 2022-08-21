using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnTeleport : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabTeleport;
    [SerializeField]
    private Vector3 spawnPrefabAt;
    [SerializeField]
    private Vector3 spawnPlayerAt;
    [SerializeField]
    private string teleportPath;
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
            Teleport = Instantiate(prefabTeleport, spawnPrefabAt, Quaternion.identity);
            Teleport.GetComponent<TeleportParty>().GetSpawnToPosition(spawnPlayerAt, teleportPath);
            Teleport.GetComponent<NetworkObject>().Spawn();
        }
        if (TeleportClient != null) return;
        if (!NetworkManager.Singleton.IsServer)
        {
            TeleportClient = GameObject.Find("Teleport(Clone)");
            TeleportClient.GetComponent<TeleportParty>().GetSpawnToPosition(spawnPlayerAt, teleportPath);
        }
    }
}
