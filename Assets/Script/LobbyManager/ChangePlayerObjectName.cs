using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using Unity.Netcode;
using TMPro;

public class ChangePlayerObjectName : NetworkBehaviour
{
    PhotonRealtimeTransport photo;
    DataCollect dataCollect = new DataCollect();
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();
        if (IsOwner)
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
    [ServerRpc]
    void ChangeServerRpc(DataCollect data)
    {
        ChangeClientRpc(data);
    }
    [ClientRpc]
    void ChangeClientRpc(DataCollect data)
    {
        this.gameObject.name = data.PlayerName;
        text.text = data.PlayerName;
    }
}
