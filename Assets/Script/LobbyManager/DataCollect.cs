using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public struct DataCollect : INetworkSerializable
{
    public string PlayerName;
    public ulong PlayerId;
    public DataCollect(string playerName, ulong playerId)
    {
        PlayerName = playerName;
        PlayerId = playerId;
    }

    public void SetDataCollect(string playerName, ulong playerId)
    {
        PlayerName = playerName;
        PlayerId = playerId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref PlayerId);
    }
}
