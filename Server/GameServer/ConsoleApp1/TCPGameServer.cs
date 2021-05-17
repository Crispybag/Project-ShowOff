using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using sharedAngy;
using System.Collections.Generic;

namespace Server
{
    class TCPGameServer
    {
        public Dictionary<TCPMessageChannel, PlayerInfo> _allConnectedUsers;

        private TestRoom _testRoom;
        private TCPGameServer()
        {
            _allConnectedUsers = new Dictionary<TCPMessageChannel, PlayerInfo>();
            _testRoom = new TestRoom(this);
            
        }

        static void Main(string[] args)
        {
            TCPGameServer tcpGameServer = new TCPGameServer();
            tcpGameServer.run();
        }

        private void run()
        {
            //Start Server
            Console.WriteLine("Start Server On Port 42069");
            TcpListener listener = new TcpListener(IPAddress.Any, 42069);
            
            //Look for max 10 clients
            listener.Start(10);

            while (true)
            {
                //check for new members	
                if (listener.Pending())
                {
                    //get the waiting client
                    Console.WriteLine("Accepting new client...");
                    TcpClient client = listener.AcceptTcpClient();

                    //=================================================
                    //Client Join Handling
                    //and wrap the client in an easier to use communication channel
                    TCPMessageChannel channel = new TCPMessageChannel(client);
                    //and add it to the login room for further 'processing'
                    _testRoom.AddMember(channel);
                    AddPlayerInfo(channel);
                }

                //=================================================
                //Update Checks
                _testRoom.Update();
                Thread.Sleep(100);
            }
        }

        public void AddPlayerInfo(TCPMessageChannel pClient)
        {
            _allConnectedUsers.Add(pClient, new PlayerInfo("John lol"));
        }

        public void RemovePlayerInfo(TCPMessageChannel pClient)
        {
            _allConnectedUsers.Remove(pClient);
        }
    }

    
}
