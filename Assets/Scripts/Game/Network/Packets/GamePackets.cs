using System;
using System.Collections.Generic;
using System.IO;

public abstract class GamePacket<T> : NetworkPacket<T>
{
    public ushort userPacketType { get; set; }
    public GamePacket(ushort userPacketType) : base((ushort)PacketType.User)
    {
        this.userPacketType = userPacketType;
    }
}

public class PositionPacket : GamePacket<PositionData>
{
    public PositionPacket() : base((ushort)UserPacketType.Position)
    {
        reliable = false;
    }

    protected override void OnDeserialize(Stream stream)
    {
        BinaryReader br = new BinaryReader(stream);
        PositionData positionData = new PositionData();
        positionData.position.x = br.ReadSingle();
        positionData.position.y = br.ReadSingle();
        positionData.position.z = br.ReadSingle();
        payload = positionData;
    }

    protected override void OnSerialize(Stream stream)
    {
        BinaryWriter bw = new BinaryWriter(stream);
        bw.Write(payload.position.x);
        bw.Write(payload.position.y);
        bw.Write(payload.position.z);
    }
}

public class LobbyPacket : GamePacket<LobbyData>
{
    public LobbyPacket() : base((ushort)UserPacketType.Lobby)
    {
        reliable = true;
    }

    protected override void OnDeserialize(Stream stream)
    {
        BinaryReader br = new BinaryReader(stream);

        LobbyData lobbyData = new LobbyData();
        lobbyData.users = br.ReadString();
        payload = lobbyData;
    }

    protected override void OnSerialize(Stream stream)
    {
        BinaryWriter bw = new BinaryWriter(stream);
        bw.Write(payload.users);
    }
}

public class StringMessagePacket : GamePacket<MessageData>
{
    public StringMessagePacket() : base((ushort)UserPacketType.Message)
    {
        reliable = true;
    }

    protected override void OnDeserialize(Stream stream)
    {
        BinaryReader br = new BinaryReader(stream);

        MessageData messageData = new MessageData();
        messageData.username = br.ReadString();
        messageData.message = br.ReadString();
        payload = messageData;
    }

    protected override void OnSerialize(Stream stream)
    {
        BinaryWriter bw = new BinaryWriter(stream);
        bw.Write(payload.username);
        bw.Write(payload.message);
    }
}