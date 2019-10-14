
public abstract class UserNetworkPacket<T> : NetworkPacket<T>
{
    public ushort userPacketType { get; private set; }

    public UserNetworkPacket(ushort userPacketType) : base((ushort)PacketType.User)
    {
        this.userPacketType = userPacketType;
    }
}