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
    }

    protected override void OnDeserialize(Stream stream)
    {
        BinaryReader br = new BinaryReader(stream);
        //payload = br.ReadString();
    }

    protected override void OnSerialize(Stream stream)
    {
        BinaryWriter bw = new BinaryWriter(stream);
        //bw.Write(payload);
    }
}

public class StringMessagePacket : GamePacket<MessageData>
{
    public StringMessagePacket() : base((ushort)UserPacketType.Message)
    {
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