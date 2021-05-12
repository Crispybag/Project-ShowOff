using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Shared;
using System.Collections.Generic;

namespace Server
{
    class TCPGameServer
    {
        public Dictionary<TcpClient, PlayerInfo> _allConnectedUsers;

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
            
            //Look for max 2 clients
            listener.Start(2);

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

                }

                //=================================================
                //Update Checks

                Thread.Sleep(100);
            }
        }

        public void RemovePlayerInfo(TcpClient pClient)
        {
            _allConnectedUsers.Remove(pClient);
        }
    }

    
}
