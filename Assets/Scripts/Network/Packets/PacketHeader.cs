using System.IO;

enum PacketType
{
    ConnectionRequest = 0,
    ChallengeRequest,
    ChallengeResponse,
    ConnectionAccepted,
    User
}

public class PacketHeader : ISerializablePacket
{
    public ushort packetType { get ; set; }
    public bool reliable { get ; set; }

    public void Serialize(Stream stream)
    {
        BinaryWriter binaryWriter = new BinaryWriter(stream);

        binaryWriter.Write(packetType);
    }

    public void Deserialize(Stream stream)
    {
        BinaryReader binaryReader = new BinaryReader(stream);

        packetType = binaryReader.ReadUInt16();
    }
}