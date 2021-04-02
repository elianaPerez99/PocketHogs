using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : MonoBehaviour {
    private const int MAX_CONNECTION = 6;
    private int port = 5701;

    private int hostID;
    private int webHostId;

    private int reliableChannel;
    private int unreliableChannel;

    private int connectionID;

    private float connectionTime;
    private bool isConnected = false;
    private bool isStarted = false;
    private byte error;
    private string ip;
    private void Update()
    {
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
                case NetworkEventType.DataEvent: break;
                case NetworkEventType.DisconnectEvent: break;

                case NetworkEventType.BroadcastEvent:

                    break;
            }
        }
        else
        {
            return; //not putting a debug log so we dont like spam it 
        }
      
    }

    public void Connect()
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
        if (((NetworkError)error).ToString() == "0")
        {
            connectionTime = Time.time;
            isConnected = true;
            //go to next scene
        }
        else 
        {
            //not allowing the client to go forward without a valid ip
            GameObject.Find("WarningText").GetComponent<Text>().text = ((NetworkError)error).ToString();
            return;
        }
        
    }

    public string TextFieldTest()
    {
        string potentialIP = GameObject.Find("IPInputField").GetComponent<InputField>().text;
        if (potentialIP == "")
        {
            return "Please enter an IP";
        }
        else
        {
            return "";
        }

    }

    public string ConnectTest(string testIP)
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topop = new HostTopology(cc, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(topop, 0);
        connectionID = NetworkTransport.Connect(hostID, testIP, port, 0, out error);
            return ((NetworkError)error).ToString();

    }

}
