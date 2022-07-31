using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public struct MessageText : INetworkSerializable
{
    public string PlayerName;
    public string Message;
    public MessageText(string playerName, string message)
    {
        PlayerName = playerName;
        Message = message;
    }

    public void SetMassage(string playerName, string message)
    {
        PlayerName = playerName;
        Message = message;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref Message);
    }
}

