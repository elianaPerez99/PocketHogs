﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
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
                case NetworkEventType.Nothing:
                    {
                        string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        Debug.Log("Recieving: " + msg);
                        break;
                    }
                case NetworkEventType.DataEvent:
                    {
                        string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        Debug.Log("Recieving: " + msg);
                        break;
                    }

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
       
    public void GetHedgeHogs(string msg)
    {
        
    }
}
