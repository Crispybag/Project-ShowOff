﻿using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using sharedAngy;
using System.Collections.Generic;

namespace Server
{
    class TCPGameServer
    {
        public Dictionary<TCPMessageChannel, PlayerInfo> allConnectedUsers;
        public Dictionary<string, Room> availableRooms;
        private LoginRoom _loginRoom;
        private LobbyRoom _lobbyRoom;
        private TCPGameServer()
        {
            allConnectedUsers = new Dictionary<TCPMessageChannel, PlayerInfo>();
            availableRooms = new Dictionary<string, Room>();
            _loginRoom = new LoginRoom(this);
            _lobbyRoom = new LobbyRoom(this);
            
            availableRooms.Add("Lobby", _lobbyRoom);
            availableRooms.Add("Login", _loginRoom);
            
            //_testRoom = new Prototype0(this);

            
        }

        static void Main(string[] args)
        {
            TCPGameServer tcpGameServer = new TCPGameServer();
            tcpGameServer.run();
        }

        private void run()
        {
            //Start Server
            Logging.LogInfo("Starting a server on port 42069", Logging.debugState.USER);
            TcpListener listener = new TcpListener(IPAddress.Any, 42069);
            
            //Look for max 10 clients
            listener.Start(10);

            while (true)
            {
                //check for new members	
                if (listener.Pending())
                {
                    //get the waiting client
                    Logging.LogInfo("Accepting new client...");
                    TcpClient client = listener.AcceptTcpClient();
                    Logging.LogInfo("Client Accepted", Logging.debugState.DETAILED);
                    
                    //=================================================
                    //Client Join Handling
                    //and wrap the client in an easier to use communication channel
                    Logging.LogInfo("Trying to wrap client in channel", Logging.debugState.DETAILED);
                    TCPMessageChannel channel = new TCPMessageChannel(client);
                    Logging.LogInfo("client wrapped in channel", Logging.debugState.DETAILED);

                    //and add it to room for processing
                    //_testRoom.AddMember(channel);
                    _loginRoom.AddMember(channel);
                    Logging.LogInfo("Client Done with processing \n\n", Logging.debugState.DETAILED);
                }

                //=================================================
                //Update Checks
                foreach(KeyValuePair<string, Room> pair in availableRooms)
                {
                    pair.Value.Update();
                }
                //_testRoom.Update();
                Thread.Sleep(100);
            }
        }



        //=================================================================
        // Player Info Tools
        //=================================================================

        public void AddPlayerInfo(TCPMessageChannel pClient, string pName)
        {
            allConnectedUsers.Add(pClient, new PlayerInfo(pName));
        }

        public void RemovePlayerInfo(TCPMessageChannel pClient)
        {
            allConnectedUsers.Remove(pClient);
        }
    }

    
}
