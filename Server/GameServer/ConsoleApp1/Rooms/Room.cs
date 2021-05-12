using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    class Room
    {
        //get access to the server
        protected TCPGameServer _server;
        
        //users currently in this room
        protected List<TcpClient> _users;

        //timer for heartbeat packets
        private int _ticks;

        //Initialize
        protected Room(TCPGameServer pServer)
        {
            _server = pServer;
            _users = new List<TcpClient>();
        }

        //add a member to this room to start receiving packages
        protected void addMember(TcpClient pListener)
        {
            _users.Add(pListener);
        }

        //remove a member from this room to stop receiving packages
        protected void removeMember(TcpClient pListener)
        {
            _users.Remove(pListener);
        }

        //delet player
        protected void removeAndCloseMember(TcpClient pMember)
        {
            removeMember(pMember);
            _server.RemovePlayerInfo(pMember);
            pMember.Close();
        }

        //check if player needs to delet
        protected void checkFaultyMember(TcpClient pClient)
        {
            if (pClient.Connected)
            removeAndCloseMember(pClient);
        }
    }
}
