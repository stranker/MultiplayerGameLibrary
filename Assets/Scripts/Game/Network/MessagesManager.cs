using System;
using UnityEngine;

public class MessageData{
    public string username;
    public string message;
}

public class MessagesManager : Singleton<MessagesManager>
{

    protected override void Initialize()
    {
        base.Initialize();
    }

    public void SendString(string message, uint senderId, uint objectId)
    {
        Debug.Log($"Sending: " + message);

        StringMessagePacket packet = new StringMessagePacket();
        MessageData messageData = new MessageData();
        messageData.username = "Test";
        messageData.message = message;

        packet.payload = messageData;

        PacketsManager.Instance.SendPacket(packet, null, senderId, objectId);
    }

    public void SendString(string username, string message, uint senderID, uint objectID)
    {
        StringMessagePacket messagePacket = new StringMessagePacket();
        MessageData messageData = new MessageData();

        messageData.username = username;
        messageData.message = message;
        messagePacket.payload = messageData;
        PacketsManager.Instance.SendPacket(messagePacket, null, senderID, objectID);
    }
}
