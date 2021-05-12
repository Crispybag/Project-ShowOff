using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Shared;

namespace Server
{
    abstract class Room
    {
        //get access to the server
        protected TCPGameServer _server;
        
        //users currently in this room
        protected List<TCPMessageChannel> _users;

        //timer for heartbeat packets
        private int _ticks;

        //Initialize
        protected Room(TCPGameServer pServer)
        {
            _server = pServer;
            _users = new List<TCPMessageChannel>();
        }

        //add a member to this room to start receiving packages
        protected void addMember(TCPMessageChannel pListener)
        {
            Console.WriteLine("User added to the room list");
            _users.Add(pListener);
        }

        //remove a member from this room to stop receiving packages
        protected void removeMember(TCPMessageChannel pListener)
        {
            _users.Remove(pListener);
        }

        //delet player
        protected void removeAndCloseMember(TCPMessageChannel pMember)
        {
            Console.WriteLine("Found a shitty client, goodbye");
            removeMember(pMember);
            _server.RemovePlayerInfo(pMember);
            pMember.Close();
        }

        //check if player needs to delet
        protected void checkFaultyMember(TCPMessageChannel pClient)
        {
            if (pClient.Connected)
            removeAndCloseMember(pClient);
        }

        //backward looping so it doesnt cry when a client gets removed from list :)
        protected void safeForEach(Action<TCPMessageChannel> pMethod)
        {
            for (int i = _users.Count - 1; i >= 0; i--)
            {
                //skip any members that have been 'killed' in the mean time
                if (i >= _users.Count) continue;
                //call the method on any still existing member
                pMethod(_users[i]);
            }
        }

        //go through all clients to see which ones should be removed
        private void snitchFaultyClients()
        {
            safeForEach(checkFaultyMember);
        }

        private void receiveAndProcessNetworkMessagesFromMember(TCPMessageChannel pMember)
        {
            while (pMember.HasMessage())
            {
                Console.WriteLine("Trying to handle a message");
                handleNetworkMessage(pMember.ReceiveMessage(), pMember);
            }
        }

        protected void receiveAndProcessNetworkMessages()
        {
            safeForEach(receiveAndProcessNetworkMessagesFromMember);
        }

        abstract protected void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender);

        protected void sendToAll(ASerializable pMessage)
        {
            foreach (TCPMessageChannel user in _users)
            {
                user.SendMessage(pMessage);
            }
        }

        private void sendPulse()
        {
            Heartbeat pulse = new Heartbeat();
            sendToAll(pulse);
        }

        public virtual void Update()
        {
            //add to timer
            _ticks++;

            //send a heartbeat packet to each client
            if (_ticks == 5) { sendPulse(); _ticks = 0; }

            snitchFaultyClients();
            receiveAndProcessNetworkMessages();
        }
    }
}
