using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyData {
    public string users;
}

public class LobbyManager : Singleton<LobbyManager>
{

    protected override void Initialize()
    {
        base.Initialize();
    }

    public void SendUsers(List<string> userList, uint senderId, uint objectId)
    {

        LobbyPacket packet = new LobbyPacket();
        LobbyData lobbyData = new LobbyData();
        string data = "";
        foreach (string user in userList)
        {
            data += user + ',';
        }
        lobbyData.users = data;
        packet.payload = lobbyData;
        PacketsManager.Instance.SendPacket(packet, null, senderId, objectId);
    }
}
