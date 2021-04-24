using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;

public class ServerClient
{
    public int connectionId;
    public string playerName;
    public Vector2 position;
    //add more to this as we have more information
}

public class ServerScript : MonoBehaviour 
{
    //networking stuff
    private List<ServerClient> clients = new List<ServerClient>();

    private const int MAX_CONNECTION = 6;
    private int port = 5701;

    private int hostID;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private bool isStarted = false;
    private byte error;

    //hedgehog stuff
    public ServerSpawner hhSpawner;
    private void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topop = new HostTopology(cc, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(topop, port, null);
        webHostId = NetworkTransport.AddWebsocketHost(topop, port, null);
        isStarted = true;
    }

    private void Update()
    {
        
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:
                SendHHData(channelId);
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("Player " + connectionId + " has connected");
                OnConnection(connectionId);
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                string[] splitData = msg.Split('|');

                switch (splitData[0])
                {
                    case "SENDPLYPOS":
                        OnSendPlayerPosition(connectionId, float.Parse(splitData[2]), float.Parse(splitData[3]));
                        Send(msg, unreliableChannel, clients);
                        break;

                    default:
                        Debug.Log("Invalid message: " + msg);
                        break;
                }

                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Player " + connectionId + " has disconnected");
                OnDisconnect(connectionId);
                break;

            case NetworkEventType.BroadcastEvent:

                break;
        }

    }

    private void OnConnection(int cnnID)
    {
        ServerClient c = new ServerClient();
        c.connectionId = cnnID;
        c.playerName = "Player " + cnnID.ToString();
        c.position = new Vector3(0, 0, 0);
        clients.Add(c);

        string msg = "NAME|" + c.playerName + "|" + c.connectionId + "|";
        foreach (ServerClient sc in clients)
        {
            msg += sc.playerName + "%" + sc.connectionId + "%" + sc.position.x + "%" + sc.position.y + "|";
        }
        msg = msg.Trim('|');
        Send(msg, reliableChannel, cnnID);

        // Send new connection so player spawns for other clients
        msg = "CNN|" + c.playerName + "|" + c.connectionId + "|" + c.position.x + "|" + c.position.y;
        Send(msg, reliableChannel, clients);
        hhSpawner.SpawnHedgeHogs(clients.Count);
    }

    private void OnDisconnect(int cnnID)
    {
        foreach (ServerClient c in clients)
        {
            if (c.connectionId == cnnID)
            {
                clients.Remove(c);
            }
        }

        // Tell everyone that someone has disconnected
        Send("DC|" + cnnID, reliableChannel, clients);
    }

    private void Send(string message, int channelId, int cnnId)
    {
        List<ServerClient> c = new List<ServerClient>();
        c.Add(clients.Find(x => x.connectionId == cnnId));
        Send(message, channelId, c);
    }

    private void Send(string message, int channelId, List<ServerClient> c)
    {
        byte[] msg = Encoding.Unicode.GetBytes(message);
        foreach (ServerClient sc in c)
        {
            NetworkTransport.Send(hostID, sc.connectionId, channelId, msg, message.Length * sizeof(char), out error);
        }
    }

    // Set position of player from data they sent
    private void OnSendPlayerPosition(int cnnID, float x, float y)
    {
        clients.Find(c => c.connectionId == cnnID).position = new Vector3(x, y, 0);
    }

    private void SendHHData(int channelId)
    {
        string message = OutputHHDataToString();
        Send(message, channelId, clients);
    }

    //hedge hog data to send
    private string OutputHHDataToString()
    {
        string msg = "HH|";
        if (hhSpawner.GetList().Count > 0)
        {
            foreach (hhData hh in hhSpawner.GetList())
            {
                msg += hh.id.ToString() + "~" + hh.position.ToString() + "~" + hh.rotation.ToString() + "~" + hh.velocity.ToString() + "`";
            }
            msg = msg.Trim('`');
        }

        return msg;
    }

}
