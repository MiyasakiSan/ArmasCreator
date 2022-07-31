using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;
using System;
using UnityEngine.UI;

public class ServerConnection : MonoBehaviour
{
    public string RoomName = "testroom";
    public string NickName = "testname";
    public Button HostButton;
    public Button JoinButton;
    // Start is called before the first frame update
    public void onHost()
    {
        GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>().RoomName = RoomName;
        GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>().NickName = NickName;
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
        HostButton.gameObject.SetActive(false);
        JoinButton.gameObject.SetActive(false);
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "1234";
        callback(true, null, approve, new Vector3(0,5,0), Quaternion.identity);
    }
    public void onJoin()
    {
        GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>().RoomName = RoomName;
        GameObject.Find("NetworkManager").GetComponent<PhotonRealtimeTransport>().NickName = NickName;
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("1234");
        NetworkManager.Singleton.StartClient();
        HostButton.gameObject.SetActive(false);
        JoinButton.gameObject.SetActive(false);
    }
}
