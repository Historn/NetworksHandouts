﻿using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;

public class ServerTCP : MonoBehaviour
{
    Socket socket;
    Thread mainThread = null;

    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string serverText;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        UItext.text = serverText;

    }


    public void startServer()
    {
        serverText = "Starting TCP Server...";

        //TO DO 1
        //Create and bind the socket
        //Any IP that wants to connect to the port 9050 with TCP, will communicate with this socket
        //Don't forget to set the socket in listening mode
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
            socket.Bind(ipep);
            socket.Listen(10);
        }
        catch
        {
            Debug.Log("Error starting server");
        }

        //TO DO 3
        //TIme to check for connections, start a thread using CheckNewConnections
        mainThread = new Thread(CheckNewConnections);
        mainThread.Start();
        
    }

    void CheckNewConnections()
    {
        while(true)
        {
            User newUser = new User();
            newUser.name = "Arnau";
            //TO DO 3
            //TCP makes it so easy to manage conections, so we are going
            //to put it to use
            //Accept any incoming clients and store them in this user.
            //When accepting, we can now store a copy of our server socket
            //who has established a communication between a
            //local endpoint (server) and the remote endpoint(client)
            //If you want to check their ports and adresses, you can acces
            //the socket's RemoteEndpoint and LocalEndPoint
            //try printing them on the console

            newUser.socket = socket.Accept();//accept the socket

            IPEndPoint clientep = (IPEndPoint)newUser.socket.RemoteEndPoint;
            serverText = serverText + "\n" + "Connected with " + clientep.Address.ToString() + " at port " + clientep.Port.ToString();

            //TO DO 5
            //For every client, we call a new thread to receive their messages. 
            //Here we have to send our user as a parameter so we can use it's socket.
            Thread newConnection = new Thread(() => Receive(newUser));
            newConnection.Start();
        }
        //This users could be stored in the future on a list
        //in case you want to manage your connections

    }

    void Receive(User user)
    {
        //TO DO 5
        //Create an infinite loop to start receiving messages for this user
        //You'll have to use the socket function receive to be able to get them.
        byte[] data = new byte[1024];
        int recv = 0;

        while (true)
        {
            try
            {
                recv = user.socket.Receive(data);
                if (recv == 0) break;

                string receivedMessage = Encoding.ASCII.GetString(data, 0, recv);
                serverText += $"\nReceived: {receivedMessage}";

                // Send a response
                Thread answer = new Thread(() => Send(user));
                answer.Start();
            }
            catch (SocketException ex)
            {
                Debug.Log($"Receive error: {ex.Message}");
            }
        }
    }

    //TO DO 6
    //Now, we'll use this user socket to send a "ping".
    //Just call the socket's send function and encode the string.
    void Send(User user)
    {
        byte[] data = Encoding.ASCII.GetBytes("Welcome to the Server!");
        try
        {
            user.socket.Send(data);
        }
        catch (SocketException ex)
        {
            Debug.Log($"Send error: {ex.Message}");
        }
    }
}
