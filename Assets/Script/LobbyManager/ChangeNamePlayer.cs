using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using Unity.Netcode;
using TMPro;

public class ChangeNamePlayer : NetworkBehaviour
{
    PhotonRealtimeTransport photo;
    DataCollect dataCollect = new DataCollect();
    // Start is called before the first frame update
    void Start()
    {
        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().OnClientConnectedCallback += ChangeNamePlayer_OnClientConnectedCallback;
        if (IsLocalPlayer)
        {
            dataCollect.SetDataCollect(photo.NickName, OwnerClientId);
            ChangeServerRpc(dataCollect);
        }
    }
    private void Update()
    {
        if (IsOwner)
        {
            dataCollect.SetDataCollect(photo.NickName, OwnerClientId);
            ChangeServerRpc(dataCollect);
        }
    }

    private void ChangeNamePlayer_OnClientConnectedCallback(ulong obj)
    {
        if (IsLocalPlayer)
        {
            dataCollect.SetDataCollect(photo.NickName, OwnerClientId);
            ChangeServerRpc(dataCollect);
        }
    }
    [ServerRpc]
    void ChangeServerRpc(DataCollect data)
    {
        ChangeClientRpc(data);
    }
    [ClientRpc]
    void ChangeClientRpc(DataCollect data)
    {
        gameObject.name = data.PlayerName;
        GetComponentsInChildren<TMP_Text>()[0].text = data.PlayerName;
    }
} 
