using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerClient
{
    public int connectionId;
    public string playerName;
    public Vector2 position;
    public bool waitingToTrade;
    public bool trading;
    public int tradingWith;
    //add more to this as we have more information
}

public class ServerScript : MonoBehaviour 
{
    //networking stuff
    private List<ServerClient> clients = new List<ServerClient>();
    private List<FoodHealth> food;
    private Dictionary<int, string> tradedHogs = new Dictionary<int, string>();
    private int foodIds = 0;

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

    // Value for position value compression
    private float compressionVal = 100;

    // Food for food spawning
    public GameObject foodPrefab;

    //debuging stuff
    public GameObject content;
    public GameObject textPrefab;
    private void Start()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topop = new HostTopology(cc, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(topop, port, null);
        webHostId = NetworkTransport.AddWebsocketHost(topop, port, null);
        food = new List<FoodHealth>();

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
                break;
            case NetworkEventType.ConnectEvent:
                ServerDebug("Player " + connectionId + " has connected");
                OnConnection(connectionId);
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                string[] splitData = msg.Split('|');

                switch (splitData[0])
                {
                    case "SENDPLYPOS":
                        OnSendPlayerPosition(connectionId, int.Parse(splitData[2]), int.Parse(splitData[3]));
                        Send(msg, reliableChannel, clients);
                        break;
                    case "FOODDROP":
                        FoodHealth fd = SpawnFood(int.Parse(splitData[1]), int.Parse(splitData[2]));
                        fd.SetId(foodIds);
                        food.Add(fd);
                        msg += '|' + fd.GetId().ToString();
                        foodIds++;
                        Send(msg, reliableChannel, clients);
                        ServerDebug("Food was dropped by Player " + connectionId.ToString());
                        break;
                    case "BOIDOWN":
                        hhSpawner.DeleteBoi(int.Parse(splitData[1]));
                        ServerDebug("Hedge hog captured");
                        break;
                    case "TRADEPEND":
                        SetTradePendingStatus(connectionId, splitData[1]);
                        CheckTwoReadyForTrade();
                        break;
                    case "TRADEHOG":
                        ReceivedHogs(splitData[1], connectionId);
                        ServerDebug("Player " + connectionId.ToString() + " sent over " + splitData[1]);
                        break;
                    default:
                        ServerDebug("Invalid message: " + msg);
                        break;
                }

                break;
            case NetworkEventType.DisconnectEvent:
                ServerDebug("Player " + connectionId + " has disconnected");
                OnDisconnect(connectionId);
                break;

            case NetworkEventType.BroadcastEvent:

                break;
        }
        if(clients.Count > 0 )
            SendHHData(channelId);
    }

    private void OnConnection(int cnnID)
    {
        ServerClient c = new ServerClient();
        c.connectionId = cnnID;
        c.playerName = "Player " + cnnID.ToString();
        c.position = new Vector3(10, 10, 0);
        clients.Add(c);

        string msg = "NAME|" + c.playerName + "|" + c.connectionId + "|";
        foreach (ServerClient sc in clients)
        {
            Vector2 position;
            position.x = CompressPosFloat(sc.position.x);
            position.y = CompressPosFloat(sc.position.y);

            msg += sc.playerName + "%" + sc.connectionId + "%" + position.x + "%" + position.y + "|";
        }
        msg = msg.Trim('|');
        Send(msg, reliableChannel, cnnID);

        // Send new connection so player spawns for other clients
        Vector2 pos;
        pos.x = CompressPosFloat(c.position.x);
        pos.y = CompressPosFloat(c.position.y);

        msg = "CNN|" + c.playerName + "|" + c.connectionId + "|" + pos.x + "|" + pos.y;
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
                break;
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
    private void OnSendPlayerPosition(int cnnID, int x, int y)
    {
        Vector2 pos;
        pos.x = DecompressPosFloat(x);
        pos.y = DecompressPosFloat(y);

        clients.Find(c => c.connectionId == cnnID).position = new Vector3(pos.x, pos.y, 0);
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
        if (hhSpawner.GetList().Count >= 0)
        {
            foreach (hhData hh in hhSpawner.GetList())
            {
                msg += hh.id.ToString() + "~" + CompressPosFloat(hh.position.x).ToString() + '~' + CompressPosFloat(hh.position.y).ToString() + '~'
                    + CompressPosFloat(hh.velocity.x).ToString() + '~' + CompressPosFloat(hh.velocity.y).ToString() + "`";
            }
            msg = msg.Trim('`');
        }

        return msg;
    }

    // Compress position data
    private int CompressPosFloat(float x)
    {
        return (int)(x * compressionVal);
    }

    // Decompress position data
    private float DecompressPosFloat(int x)
    {
        return (float)x / compressionVal;
    }

    // Spawns food drops from other players
    private FoodHealth SpawnFood(int x, int y)
    {
        GameObject foob = Instantiate(foodPrefab) as GameObject;

        // Set food object position
        Vector3 position = Vector3.zero;
        position.x = DecompressPosFloat(x);
        position.y = DecompressPosFloat(y);
        foob.transform.position = position;
        foob.GetComponent<FoodHealth>().SetServer(gameObject);
        return foob.GetComponent<FoodHealth>();
    }

    public void FoodEaten(int id)
    {
        string msg = "FOODGONE|" + id.ToString();
        Send(msg, reliableChannel, clients);
    }

    //get trade pending data
    private void SetTradePendingStatus(int id, string msg)
    {
        bool startTrade = int.Parse(msg) != 0;
        for (int i=0; i<clients.Count; i++)
        {
            if (id == clients[i].connectionId)
            {
                clients[i].waitingToTrade = startTrade;

            }
        }
    }

    private void CheckTwoReadyForTrade()
    {
        //check if two are ready
        List<int> indexsReady = new List<int>();
        for(int i=0; i<clients.Count; i++)
        {
            if (clients[i].waitingToTrade)
            {
                indexsReady.Add(i);
                
            }
        }
        if (indexsReady.Count == 2)
        {
            //telling clients that they are trading AND setting it up so the server knows who is trading
            Send("TRADESTART|", reliableChannel, clients[indexsReady[0]].connectionId);
            clients[indexsReady[0]].trading = true;
            clients[indexsReady[0]].waitingToTrade = false;
            clients[indexsReady[0]].tradingWith = clients[indexsReady[1]].connectionId;
            Send("TRADESTART|", reliableChannel, clients[indexsReady[1]].connectionId);
            clients[indexsReady[1]].trading = true;
            clients[indexsReady[1]].waitingToTrade = false;
            clients[indexsReady[1]].tradingWith = clients[indexsReady[0]].connectionId;
        }
    }

    private void ReceivedHogs(string name, int id)
    {
        foreach (ServerClient sc in clients)
        {
            //finding who sent it
            if (sc.connectionId == id)
            {
                tradedHogs.Add(id, name);
                if (tradedHogs.ContainsKey(sc.tradingWith))
                {
                    SendHogsBack(name, sc.tradingWith);
                    SendHogsBack(tradedHogs[sc.tradingWith], id);
                    sc.trading = false;
                    //finding who is receiving it
                    foreach (ServerClient client in clients)
                    {
                        if (client.connectionId == sc.tradingWith)
                        {
                            client.trading = false;
                            client.tradingWith = 0;
                            break;
                        }
                    }
                    //clearing dictionary
                    tradedHogs.Remove(sc.connectionId);
                    tradedHogs.Remove(sc.tradingWith);
                    sc.tradingWith = 0;
                }
                break;
            }
        }

    }

    private void SendHogsBack(string name, int id)
    {
        Send("RECEIVEHOGS|" + name, reliableChannel, id);
    }

    private void ServerDebug(string message)
    {
        GameObject temp = GameObject.Instantiate(textPrefab, content.transform);
        temp.GetComponent<Text>().text = message;
    }
}
