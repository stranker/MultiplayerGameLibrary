using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rigid;
    public float speed = 0.5f;
    private Vector2 velocity;

    void OnEnable()
    {
        PacketsManager.Instance.AddListener(0, OnReceivePacket);
    }

    private void OnReceivePacket(ushort userPacketType, uint senderId, Stream stream)
    {
        if (userPacketType != (ushort)UserPacketType.Position)
            return;
        Debug.Log("OnReceivePacket: " + userPacketType);
        ProcessPositionPacket(senderId, stream);
    }

    private void ProcessPositionPacket(uint senderId, Stream stream)
    {
        PositionPacket packet = new PositionPacket();
        packet.Deserialize(stream);
        if (!NetworkManager.Instance.isServer)
        {
            transform.position = new Vector3(packet.payload.position.x, packet.payload.position.y, packet.payload.position.z);
        }
    }

    void OnDisable()
    {
        PacketsManager.Instance.RemoveListener(0);
    }

    private void FixedUpdate()
    {
        if (NetworkManager.Instance.isServer)
        {
            velocity = Vector2.one * speed;
            rigid.velocity = velocity;
            PositionsManager.Instance.SendPosition(transform.position, 0, 0);
        }
    }

}
