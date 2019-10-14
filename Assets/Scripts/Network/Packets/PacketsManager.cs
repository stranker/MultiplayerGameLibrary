using System.IO;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PacketsManager : Singleton<PacketsManager>, IReceiveData
{
    private Dictionary<uint, System.Action<ushort,uint, Stream>> onPacketReceived = new Dictionary<uint, System.Action<ushort,uint, Stream>>();
    private uint currentPacketId = 0;

    protected override void Initialize()
    {
        base.Initialize();
        NetworkManager.Instance.onReceiveEvent += OnReceiveData;
    }

    public void AddListener(uint ownerId, System.Action<ushort, uint, Stream> callback)
    {
        if (!onPacketReceived.ContainsKey(ownerId))
            onPacketReceived.Add(ownerId, callback);
    }

    public void RemoveListener(uint ownerId)
    {
        if (onPacketReceived.ContainsKey(ownerId))
            onPacketReceived.Remove(ownerId);
    }

    public void SendPacket<T>(NetworkPacket<T> networkPacket, IPEndPoint ipEndPoint = null, uint senderId = 0, uint objectId = 0)
    {
        byte[] bytes = SerializePacket(networkPacket, senderId, objectId);

        if (NetworkManager.Instance.isServer)
        {
            if (ipEndPoint != null)
                NetworkManager.Instance.SendToClient(bytes, ipEndPoint);
            else
                NetworkManager.Instance.Broadcast(bytes);
        }
        else
            NetworkManager.Instance.SendToServer(bytes);
    }

    private byte[] SerializePacket<T>(NetworkPacket<T> networkPacket, uint senderId, uint objectId)
    {
        
        PacketHeader packetHeader = new PacketHeader();
        MemoryStream stream = new MemoryStream();

        packetHeader.packetType = networkPacket.packetType;

        packetHeader.Serialize(stream);

        if ((PacketType)networkPacket.packetType == PacketType.User)
        {
            GamePacket<T> gamePacket = networkPacket as GamePacket<T>;
            UserPacketHeader userPacketHeader = new UserPacketHeader();

            userPacketHeader.id = currentPacketId++;
            userPacketHeader.senderId = senderId;
            userPacketHeader.objectId = objectId;
            userPacketHeader.packetType = gamePacket.userPacketType;

            userPacketHeader.Serialize(stream);
        }

        networkPacket.Serialize(stream);

        stream.Close();

        return stream.ToArray();
    }

    public void OnReceiveData(byte[] data, IPEndPoint ipEndpoint)
    {
        Debug.Log($"Receiving: " + data);

        PacketHeader packetHeader = new PacketHeader();
        UserPacketHeader userPacketHeader = new UserPacketHeader();
        MemoryStream stream = new MemoryStream(data);

        DeserializePacket(data, out stream, out packetHeader, ref userPacketHeader);

        if (userPacketHeader != null)
        {
            InvokeCallback(userPacketHeader.objectId, userPacketHeader.packetType, userPacketHeader.senderId, stream);
        }

        stream.Close();
    }

    private void DeserializePacket(byte[] data, out MemoryStream stream, out PacketHeader packetHeader, ref UserPacketHeader userPacketHeader)
    {
        stream = new MemoryStream(data);
        packetHeader = new PacketHeader();

        packetHeader.Deserialize(stream);

        if ((PacketType)packetHeader.packetType == PacketType.User)
        {
            userPacketHeader = new UserPacketHeader();
            userPacketHeader.Deserialize(stream);
        }
    }

    void InvokeCallback(uint objectId, ushort userPacketType, uint senderId, Stream stream)
    {
        if (onPacketReceived.ContainsKey(objectId))
            onPacketReceived[objectId].Invoke(userPacketType, senderId, stream);
    }
}
