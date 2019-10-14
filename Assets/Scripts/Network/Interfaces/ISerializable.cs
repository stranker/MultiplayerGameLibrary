using System.IO;

public interface ISerializablePacket
{
    ushort packetType { get; set; }
    bool reliable { get; set; }

    void Serialize(Stream stream);
    void Deserialize(Stream stream);
}