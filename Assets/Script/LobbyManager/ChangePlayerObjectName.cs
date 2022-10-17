using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.PhotonRealtime;
using Unity.Netcode;
using TMPro;
using ArmasCreator.GameMode;
using ArmasCreator.Utilities;
using ArmasCreator.Gameplay.UI;

public class ChangePlayerObjectName : NetworkBehaviour
{
    PhotonRealtimeTransport photo;
    DataCollect dataCollect = new DataCollect();
    private TextMeshProUGUI playerNametext;

    private GameModeController gameModeController;
    private UIPlayerController uiPlayerController;

    private bool isSinglePlayer => gameModeController.IsSinglePlayerMode;

    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
        uiPlayerController = GameObject.Find("PlayerCanvas").GetComponent<UIPlayerController>();

        playerNametext = uiPlayerController.PlayerNameText;
    }

    void Start()
    {
        if (isSinglePlayer)
        {
            playerNametext.text = PlayerPrefs.GetString("PName");
            return;
        }

        photo = GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>();

        if (IsOwner)
        {
            dataCollect.SetDataCollect(photo.NickName, OwnerClientId);
            ChangeServerRpc(dataCollect);
        }
    }
    private void Update()
    {
        //if (IsOwner)
        //{
        //    dataCollect.SetDataCollect(photo.NickName, OwnerClientId);
        //    ChangeServerRpc(dataCollect);
        //}
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
        playerNametext.text = data.PlayerName;
    }
}
