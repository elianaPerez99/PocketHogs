using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//THIS IS DIRECTLY FROM THE TUTORIAL AKA NOT FINAL
public class ClientTestScript : MonoBehaviour {

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
    private string playerName;
    private void Update()
    {
        if (!isConnected)
            return;
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

    public void Connect()
    {
        //does player have name?
        string pName = GameObject.Find("InputField").GetComponent<InputField>().text;
        if(pName == "")
        {
            Debug.Log("enter name plz");
            return;
        }
        playerName = pName;
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topop = new HostTopology(cc, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(topop, 0);
        connectionID = NetworkTransport.Connect(hostID, "127.0.0.1", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;
    }

}
