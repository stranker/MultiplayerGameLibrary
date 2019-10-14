using System.IO;


public abstract class NetworkPacket<P> : ISerializablePacket
{
    public ushort packetType { get; set; }
    public bool reliable { get ; set; }
    public P payload;

    public NetworkPacket(ushort type)
    {
        this.packetType = type;
    }

    public virtual void Serialize(Stream stream)
    {
        OnSerialize(stream);
    }

    public virtual void Deserialize(Stream stream)
    {
        OnDeserialize(stream);
    }

    protected abstract void OnSerialize(Stream stream);
    protected abstract void OnDeserialize(Stream stream);
}

