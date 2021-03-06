using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using sharedAngy;

namespace Server
{
    public abstract class Room
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

       


        #region adding and removing members
        //=================================================================
        // Handle Adding and Removing Members
        //=================================================================

        //add a member to this room to start receiving packages
        virtual public void AddMember(TCPMessageChannel pListener)
        {
            Logging.LogInfo("adding client to room");
            _users.Add(pListener);
        }

        //remove a member from this room 
        virtual public void RemoveMember(TCPMessageChannel pListener)
        {
            _users.Remove(pListener);

        }

        //delet player
        protected void removeAndCloseMember(TCPMessageChannel pMember)
        {
            Console.WriteLine("Found a shitty client, goodbye");
            RemoveMember(pMember);
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
        virtual protected void sendPulse()
        {
            Heartbeat pulse = new Heartbeat();
            sendToAll(pulse);
        }
        #endregion

        #region packet management
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
        #endregion

        #region sending messages
        //=================================================================
        // Sending Messages
        //=================================================================
        //Send Message to all clients
        public void sendToAll(ASerializable pMessage)
        {
            foreach (TCPMessageChannel user in _users)
            {
                user.SendMessage(pMessage);
            }
        }
        
        

        //Send message to single client in room
        public void sendToUser(ASerializable pMessage, TCPMessageChannel pReceiver)
        {
            pReceiver.SendMessage(pMessage);
        }
        #endregion

        #region update & tools
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

        /// <summary>
        /// (Ezra) Call this to confirm the players characters when entering the game
        /// </summary>
        protected void sendConfPlayer(TCPMessageChannel pClient)
        {
            if (_server.allConnectedUsers[pClient].GetPlayerIndex() == 0)
            {
                //player 1 is nuc
                ConfPlayer newPlayer = new ConfPlayer();
                newPlayer.playerName = "Player1";
                pClient.SendMessage(newPlayer);
            }
            else
            {
                //player 2 is alex
                ConfPlayer newPlayer = new ConfPlayer();
                newPlayer.playerName = "Player2";
                pClient.SendMessage(newPlayer);
            }
        }
        #endregion
    }
}
