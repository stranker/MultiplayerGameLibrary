using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviourSingleton<Lobby>
{
    public Text lobby;
    // Start is called before the first frame update
    protected override void Initialize()
    {
        base.Initialize();
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        PacketsManager.Instance.AddListener(0, OnReceivePacket);
    }

    private void OnReceivePacket(ushort userPacketType, uint senderId, Stream stream)
    {
        if (userPacketType != (ushort)UserPacketType.Lobby)
            return;
        Debug.Log("OnReceivePacket: " + userPacketType);
        ProcessLobbyPacket(senderId, stream);
    }

    private void ProcessLobbyPacket(uint senderId, Stream stream)
    {
        LobbyPacket packet = new LobbyPacket();
        packet.Deserialize(stream);
        string users = packet.payload.users;
        lobby.text = "";
        string[] usersProcessed = users.Split(',');
        foreach (string user in usersProcessed)
        {
            lobby.text += user + System.Environment.NewLine;
        }
    }

    void OnDisable()
    {
        PacketsManager.Instance.RemoveListener(0);
    }
}
