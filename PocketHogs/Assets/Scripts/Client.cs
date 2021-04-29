using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering;
using System.Text;
using System.Runtime.InteropServices;

public class Player
{
    public string playerName;
    public GameObject avatar;
    public int conncetionId;
}

public class Client : MonoBehaviour {
    private const int MAX_CONNECTION = 6;
    private int port = 5701;

    private int hostID;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private int myClientId;
    private int connectionID;

    private float connectionTime;
    private bool isConnected = false;
    private bool isStarted = false;
    private byte error;
    private string ip;

    // Players information
    public GameObject playerPrefab;
    public Dictionary<int,Player> players = new Dictionary<int,Player>();

    // Information for regularly keeping track of player movement updates
    private float lastMovementUpdate;
    private float movementUpdateRate = 0.05f;

    //hh spawner 
    HedgehogSpawner spawner;
    bool hasSpawner = false;

    // Value for position value compression
    private float compressionVal = 100;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Map1") && !hasSpawner)
        {
            spawner = GameObject.Find("HedgeHogSpawner").GetComponent<HedgehogSpawner>();
            hasSpawner = true;
        }
        //only running update information if the client has already connected
        if (isConnected)
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
                case NetworkEventType.Nothing: break;
                case NetworkEventType.ConnectEvent: break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    string[] splitData = msg.Split('|');

                    switch(splitData[0])
                    {
                        case "NAME":
                            Debug.Log("Client join message");
                            OnJoin(splitData);
                            break;

                        case "CNN":
                            Debug.Log("Client connect message");
                            SpawnPlayer(splitData[1], int.Parse(splitData[2]), int.Parse(splitData[3]), int.Parse(splitData[4]));
                            break;

                        case "DC":
                            Debug.Log("Client disconnect message");
                            PlayerDisconnected(int.Parse(splitData[1]));
                            break;

                        case "SENDPLYPOS":
                            OnSendPlayerPosition(splitData);
                            break;
                        case "HH":
                            if (hasSpawner)
                            {
                                GetHedgeHogs(splitData[1], Time.deltaTime);
                            }
                            break;
                        default:
                            Debug.Log("Invalid message: " + msg);
                            break;
                    }

                    break;
                case NetworkEventType.DisconnectEvent: break;

                case NetworkEventType.BroadcastEvent:

                    break;
            }

            // Get my player for position sending checks
            GameObject myPlayer = players[myClientId].avatar;

            // Get player positions
            if (myPlayer.GetComponent<PlayerMovement>().HasMoved() && (Time.time - lastMovementUpdate > movementUpdateRate))
            {
                // Reset time check frame
                lastMovementUpdate = Time.time;
                myPlayer.GetComponent<PlayerMovement>().SetLastPos();

                // Compress position data
                Vector2 pos;
                pos.x = CompressPosFloat(myPlayer.transform.position.x);
                pos.y = CompressPosFloat(myPlayer.transform.position.y);

                // Put together message to set and request locations
                string msg = "SENDPLYPOS|" + myClientId + '|' + pos.x + '|' + pos.y;

                // Send out my position
                Send(msg, reliableChannel);
            }
        }
        else
        {
            return; //not putting a debug log so we dont like spam it 
        }
      
    }

    public void Connect()
    {
        //make sure we can only connect once
        if (!isConnected)
        {
            //making sure that the ip is correct
            string potentialIP = GameObject.Find("IPInputField").GetComponent<InputField>().text;
            if (potentialIP == "")
            {
                GameObject.Find("WarningText").GetComponent<Text>().text = "Please enter an IP";
                return;
            }
            ip = potentialIP;
            NetworkTransport.Init();
            ConnectionConfig cc = new ConnectionConfig();

            reliableChannel = cc.AddChannel(QosType.Reliable);
            unreliableChannel = cc.AddChannel(QosType.Unreliable);

            HostTopology topop = new HostTopology(cc, MAX_CONNECTION);

            hostID = NetworkTransport.AddHost(topop, 0);
            connectionID = NetworkTransport.Connect(hostID, ip, port, 0, out error);
            if (((NetworkError)error).ToString() == "Ok")
            {
                connectionTime = Time.time;
                isConnected = true;
                SceneManager.LoadScene("Map1");
            }
            else
            {
                //not allowing the client to go forward without a valid ip
                GameObject.Find("WarningText").GetComponent<Text>().text = ((NetworkError)error).ToString();
                return;
            }

        }
    }

    private void Send(string message, int channelId)
    {
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, channelId, msg, message.Length * sizeof(char), out error);
    }

    // Need to spawn in players on join
    private void OnJoin(string[] data)
    {
        // Set my client id
        myClientId = int.Parse(data[2]);

        // Spawn all players currently in game
        for(int i = 3; i < data.Length -1; i++)
        {
            string[] d = data[i].Split('%');
            SpawnPlayer(d[0], int.Parse(d[1]), int.Parse(d[2]), int.Parse(d[3]));
        }
    }

    // Spawn in players
    private void SpawnPlayer(string playerName, int cnnId, int x, int y)
    {
        // Spawn player object
        Debug.Log("Spawn player " + cnnId);
        GameObject go = Instantiate(playerPrefab) as GameObject;

        // Set player object position
        Vector3 position = Vector3.zero;
        position.x = DecompressPosFloat(x);
        position.y = DecompressPosFloat(y);
        go.transform.position = position;

        // If player is me, let me be moveable and have hog pockets
        if(cnnId == myClientId)
        {
            go.GetComponent<Rigidbody>().isKinematic = false;
            go.AddComponent<HogPockets>();
            isStarted = true;
        }
        // If no me, remove the movement controller
        else
        {
            Destroy(go.GetComponent<PlayerMovement>());
        }

        // Create new player data to add to the player dictionary
        Player p = new Player();
        p.avatar = go;
        p.playerName = playerName;
        players.Add(cnnId,p);
    }

    // Set position of players and send my own
    private void OnSendPlayerPosition(string[] data)
    {
        // If not me, then parse position data
        if (int.Parse(data[1]) != myClientId)
        {
            // Parsing position data from message
            Vector3 position = Vector3.zero;
            position.x = DecompressPosFloat(int.Parse(data[2]));
            position.y = DecompressPosFloat(int.Parse(data[3]));

            // Set given player's position to parsed position
            players[int.Parse(data[1])].avatar.transform.position = position;
        }
    }

    // Remove date of player who has disconnected
    private void PlayerDisconnected(int cnnId)
    {
        Destroy(players[cnnId].avatar);
        players.Remove(cnnId);
    }

    //update the hedge hogs locations
    public void GetHedgeHogs(string msg, float sentTime)
    {
        spawner.UpdateHogs(msg, sentTime);
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
}
