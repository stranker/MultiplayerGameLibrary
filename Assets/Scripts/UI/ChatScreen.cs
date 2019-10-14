using UnityEngine.UI;
using System.IO;
using UnityEngine;
using System;
using System.Net;
using System.Collections.Generic;

public class ChatScreen : MonoBehaviourSingleton<ChatScreen>
{
    public Text messages;
    public InputField inputMessage;
    public Text lobby;
    public string username = "";

    protected override void Initialize()
    {
        base.Initialize();
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        PacketsManager.Instance.AddListener(0, OnReceivePacket);
    }

    void OnDisable()
    {
        PacketsManager.Instance.RemoveListener(0);
    }

    void OnReceivePacket(ushort userPacketType, uint senderId, Stream stream)
    {
        if (userPacketType != (ushort)UserPacketType.Message)
            return;
        Debug.Log("OnReceivePacket: " + userPacketType);
        ProcessMessage(senderId, stream);
    }

    private void ProcessMessage(uint senderId, Stream stream)
    {
        StringMessagePacket packet = new StringMessagePacket();
        packet.Deserialize(stream);
        string username = packet.payload.username;
        string message = packet.payload.message;

        if (NetworkManager.Instance.isServer)
        {
            MessagesManager.Instance.SendString(username, message, senderId, 0);
        }
        messages.text += username + ":" + message + System.Environment.NewLine;
    }

    public void SetUsername(string user)
    {
        username = user;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (inputMessage && inputMessage.text != "")
            {
                if (NetworkManager.IsAvailable())
                {
                    MessagesManager.Instance.SendString(username, inputMessage.text, 0, 0);
                    if (NetworkManager.Instance.isServer)
                    {
                        messages.text += "Server: " + inputMessage.text + System.Environment.NewLine;
                    }
                }
                inputMessage.ActivateInputField();
                inputMessage.Select();
                inputMessage.text = "";
            }
        }
    }
}
