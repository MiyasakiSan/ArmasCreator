using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using Unity.Netcode;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using TMPro;
using Netcode.Transports.PhotonRealtime;
using ArmasCreator.Utilities;
using ArmasCreator.GameMode;

public class CreateRoom : MonoBehaviour
{
    public string roomName = "Test";
    public string nickName = "TestName";
    public string sceneName;
    public TMP_Text[] RoomID;
    public TMP_Text[] RoomPW;
    PhotonRealtimeTransport transport;
    GameObject[] NetworkManagers;

    private GameModeController gameModeController;
    // Happen on server
    private void Awake()
    {
        gameModeController = SharedContext.Instance.Get<GameModeController>();
    }

    public void Start()
    {
        nickName = PlayerPrefs.GetString("PName");
        NetworkManagers = GameObject.FindGameObjectsWithTag("NetworkManager");

        if (NetworkManagers.Length > 1)
        {
            Destroy(NetworkManagers[1]);
        }
    }

    public void Host()
    {
        
        GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>().RoomName = RoomID[0].text;
        GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>().NickName = nickName;

        NetworkManager.Singleton.NetworkConfig.PlayerPrefab.name = "PlayerInfoBase(Clone)";
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        gameModeController.SelectMode(GameModeController.GameModes.MultiPlayer);
    }

    // Happen on server
    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        //Check incoming data
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == RoomPW[0].text;
        callback(true, null, approve, GetRandomSpawn(), Quaternion.identity);
    }

    public void Join()
    {
        transport = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>();
        transport.RoomName = RoomID[1].text;
        transport.NickName = nickName;
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(RoomPW[1].text);
        NetworkManager.Singleton.StartClient();

        gameModeController.SelectMode(GameModeController.GameModes.MultiPlayer);
    }
    Vector3 GetRandomSpawn()
    {
        float x = UnityEngine.Random.Range(-10f, 10f);
        float y = 4f;
        float z = UnityEngine.Random.Range(-10f, 10f);
        return new Vector3(x, y, z);
    }
    public void RoomNameChanged(string newRoomName)
    {
        this.roomName = newRoomName;
    }
}

