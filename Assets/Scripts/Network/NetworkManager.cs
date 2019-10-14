using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;

public struct Client
{
    public float lastMsgTimeStamp;
    public uint id;
    public IPEndPoint ipEndPoint;

    public Client(IPEndPoint ipEndPoint, uint id, float timeStamp)
    {
        this.lastMsgTimeStamp = timeStamp;
        this.id = id;
        this.ipEndPoint = ipEndPoint;
    }
}

public class NetworkManager : MonoBehaviourSingleton<NetworkManager>, IReceiveData
{
    public IPAddress ipAddress
    {
        get; private set;
    }

    public int port
    {
        get; private set;
    }

    public bool isServer
    {
        get; private set;
    }

    public int timeOut = 30;

    public Action<byte[], IPEndPoint> onReceiveEvent;

    private UdpConnection connection;

    private List<string> userList = new List<string>();
    private readonly Dictionary<uint, Client> clients = new Dictionary<uint, Client>();
    private readonly Dictionary<IPEndPoint, uint> ipToId = new Dictionary<IPEndPoint, uint>();

    // This id should be generated during first handshake
    public uint clientId
    {
        get; private set;
    }

    public bool StartServer(int port)
    {
        try
        {
            isServer = true;
            this.port = port;
            connection = new UdpConnection(port, this);
            Debug.Log("Starting server at port: " + port.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }

        return true;
    }

    public Dictionary<uint, Client> GetClients()
    {
        return clients;
    }

    public void AddUser(string username)
    {
        if (!userList.Contains(username))
        {
            userList.Add(username);
        }
    }

    public List<string> GetUserList()
    {
        return userList;
    }

    public bool StartClient(IPAddress ip, int port, string username)
    {
        try
        {
            isServer = false;
            this.port = port;
            this.ipAddress = ip;
            connection = new UdpConnection(ip, port, this);
            AddClient(new IPEndPoint(ip, port));
            ChatScreen.Instance.SetUsername(username);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }

        return true;
    }

    private void AddClient(IPEndPoint ip)
    {
        if (!ipToId.ContainsKey(ip))
        {
            Debug.Log("Adding client: " + ip.Address);
            uint id = clientId;
            ipToId[ip] = clientId;
            clients.Add(clientId, new Client(ip, id, Time.realtimeSinceStartup));
            clientId++;
        }
    }

    void RemoveClient(IPEndPoint ip)
    {
        if (ipToId.ContainsKey(ip))
        {
            Debug.Log("Removing client: " + ip.Address);
            clients.Remove(ipToId[ip]);
        }
    }

    public void OnReceiveData(byte[] data, IPEndPoint ip)
    {
        AddClient(ip);
        if (onReceiveEvent != null)
            onReceiveEvent.Invoke(data, ip);
    }

    public void SendToServer(byte[] data)
    {
        Debug.Log("Sending to server");
        connection.Send(data);
    }

    public void Broadcast(byte[] data)
    {
        Debug.Log($"Broadcasting to: {clients.Count}");
        using (var iterator = clients.GetEnumerator())
        {
            while (iterator.MoveNext())
            {
                connection.Send(data, iterator.Current.Value.ipEndPoint);
            }
        }
    }

    public void SendToClient(byte[] data, IPEndPoint ipEndPoint)
    {
        foreach (var ip in ipToId)
        {
            if (ip.Key == ipEndPoint)
            {
                connection.Send(data, clients[ip.Value].ipEndPoint);
                break;
            }
        }
    }

    void Update()
    {
        if (connection != null)
            connection.FlushReceiveData();
    }
}
