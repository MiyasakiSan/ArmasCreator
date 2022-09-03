using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using Netcode.Transports.PhotonRealtime;

public class ChatScript : NetworkBehaviour
{
    [SerializeField]
    private TMP_InputField textInput;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private TMP_Text prefabText;
    [SerializeField]
    private Scrollbar scrollbar;
    MessageText messageText = new MessageText();
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            string Nickname = NetworkManager.Singleton.GetComponent<PhotonRealtimeTransport>().NickName;
            messageText.SetMassage(Nickname, textInput.text);
            SendTextServerRpc(messageText);
            scrollbar.value = 0;
            textInput.text = "";
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
        content.sizeDelta += new Vector2(0, 50);
        prefabText.text = messageText.PlayerName + " : " + messageText.Message;
        Instantiate(prefabText, content);
        
    }

}
