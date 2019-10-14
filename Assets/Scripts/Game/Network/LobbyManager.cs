using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{


    /*
    public void UserConnected(IPEndPoint ip, uint clientId, uint objectId)
    {
        Debug.Log("A user has connected: " + ip.Address.ToString());

        UserConnectedPacket packet = new UserConnectedPacket();

        packet.payload = clientId.ToString() + "|" + ip.Address.ToString();

        PacketsManager.Instance.SendPacket(packet, objectId);
    }

    internal void UserConnected(Dictionary<uint, Client> clients, uint objectId)
    {
        Debug.Log("A user has connected");

        UserConnectedPacket packet = new UserConnectedPacket();

        foreach (uint clientId in clients.Keys)
        {
            string info = clientId.ToString() + ":" + clients[clientId].ipEndPoint.Address.ToString() + ";";
            packet.payload += info;
        }
        PacketsManager.Instance.SendPacket(packet, objectId);
    }*/
}
