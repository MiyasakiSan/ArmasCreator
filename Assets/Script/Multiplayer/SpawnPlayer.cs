using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;

public class SpawnPlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    private GameObject prefap;
    public GameObject StagePrefap;
    IReadOnlyList<ulong> Clientlist;
    PhotonRealtimeTransport photo;
    public string PlayerName;
    public ulong PlayerId;
    public Transform[] SpawnPoints;
    public int spawncount = 0;
    void Start()
    {
        Clientlist = NetworkManager.Singleton.ConnectedClientsIds;
        if (IsOwnedByServer)
        {
            foreach (ulong ClientId in Clientlist)
            {
                getphotonServerRpc();
                ChangeServerRpc(ClientId);
                spawncount++;
            }
        }
    }
    [ServerRpc]
    void getphotonServerRpc()
    {
        getphotonClientRpc();
    }
    [ClientRpc]
    void getphotonClientRpc()
    {
        photo = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
        PlayerName = photo.NickName;
    }
    [ServerRpc]
    void ChangeServerRpc(ulong ClientId)
    {
        prefap = Instantiate(StagePrefap, GetRandomSpawn(), Quaternion.identity);
        prefap.GetComponent<NetworkObject>().SpawnAsPlayerObject(ClientId, true);
        prefap.name = PlayerName;
    }
    Vector3 GetRandomSpawn()
    {
        int Rand = Random.Range(0, SpawnPoints.Length-1);
        Vector3 RandPos = SpawnPoints[Rand].position;
        return RandPos;
    }
}