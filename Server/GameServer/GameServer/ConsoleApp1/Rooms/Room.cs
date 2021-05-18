using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using sharedAngy;

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


        //=================================================================
        // Handle Adding and Removing Members
        //=================================================================

        //add a member to this room to start receiving packages
        protected void addMember(TCPMessageChannel pListener)
        {
            Console.WriteLine("User added to the room list");
            _users.Add(pListener);
        }

        //remove a member from this room 
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
            if (!pClient.Connected)

            removeAndCloseMember(pClient);
        }

        //go through all clients to see which ones should be removed
        private void snitchFaultyClients()
        {
            safeForEach(checkFaultyMember);
        }

        //Send a pulse to a client to get an update from the connection
        private void sendPulse()
        {
            Heartbeat pulse = new Heartbeat();
            sendToAll(pulse);
        }

        
        //=================================================================
        // Packet Management
        //=================================================================
        
        //Check if a client has a message and process it
        private void receiveAndProcessNetworkMessagesFromMember(TCPMessageChannel pMember)
        {
            while (pMember.HasMessage())
            {
                Logging.LogInfo("Trying to handle message", Logging.debugState.SPAM);
                handleNetworkMessage(pMember.ReceiveMessage(), pMember);
            }
        }

        //Super function to handle network messages from all clients that have a message
        protected void receiveAndProcessNetworkMessages()
        {
            safeForEach(receiveAndProcessNetworkMessagesFromMember);
        }

        //Class to override in rooms to handle all the type of messages they can expect to receive in that room
        abstract protected void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender);

        //Send Message to all clients
        protected void sendToAll(ASerializable pMessage)
        {
            foreach (TCPMessageChannel user in _users)
            {
                user.SendMessage(pMessage);
            }
        }


        //=================================================================
        // Sending Messages
        //=================================================================

        //Send message to single client in room
        protected void sendToUser(ASerializable pMessage, TCPMessageChannel pReceiver)
        {
            pReceiver.SendMessage(pMessage);
        }


        //=================================================================
        // Update
        //=================================================================
        public virtual void Update()
        {
            //add to timer
            _ticks++;

            //send a heartbeat packet to each client
            if (_ticks == 5) { sendPulse(); _ticks = 0; }

            snitchFaultyClients();
            receiveAndProcessNetworkMessages();
        }

        //=================================================================
        // Tools
        //=================================================================

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

    }
}
