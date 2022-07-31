using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;

public class ChatScript : NetworkBehaviour
{
    public TMP_InputField TextInput;
    public RectTransform Content;
    public TMP_Text PrefabText;
    public Scrollbar scrollbar;
    MessageText messageText = new MessageText();
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            string Nickname = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>().NickName;
            messageText.SetMassage(Nickname, TextInput.text);
            SendTextServerRpc(messageText);
            scrollbar.value = 0;
            TextInput.text = "";
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SendTextServerRpc(MessageText messageText)
    {
        SendTextClientRpc(messageText);
    }
    [ClientRpc]
    public void SendTextClientRpc(MessageText messageText)
    {
        Content.sizeDelta += new Vector2(0, 50);
        PrefabText.text = messageText.PlayerName + " : " + messageText.Message;
        Instantiate(PrefabText, Content);
        
    }

}
