using System;
using System.Collections.Generic;
using System.IO;

public abstract class GameNetworkPacket<T> : NetworkPacket<T>
{
    public GameNetworkPacket(UserPacketType type) : base((ushort)type)
    {
    }
}

public class PositionPacket : GameNetworkPacket<Dictionary<string,float>>
{
    public PositionPacket() : base(UserPacketType.Position)
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

public class StringMessagePacket : GameNetworkPacket<MessageData>
{
    public StringMessagePacket() : base(UserPacketType.Message)
    {
    }

    protected override void OnDeserialize(Stream stream)
    {
        BinaryReader br = new BinaryReader(stream);

        MessageData messageData = new MessageData();
        messageData.username = br.ReadString();
        messageData.message = br.ReadString();
    }

    protected override void OnSerialize(Stream stream)
    {
        BinaryWriter bw = new BinaryWriter(stream);
        bw.Write(payload.username);
        bw.Write(payload.message);
    }
}