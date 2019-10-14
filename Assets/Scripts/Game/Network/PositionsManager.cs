using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData
{
    public Vector3 position;
}

public class PositionsManager : Singleton<PositionsManager>
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    public void SendPosition(Vector3 position, uint senderId, uint objectId)
    {
        PositionPacket packet = new PositionPacket();
        PositionData positionData = new PositionData();

        packet.payload = positionData;

        PacketsManager.Instance.SendPacket(packet, null, senderId, objectId);
    }
}
