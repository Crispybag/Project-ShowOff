using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Room where players join before the game starts, where they can chat with eachother
    /// </summary>
    class LobbyRoom : Room
    {
        public LobbyRoom(TCPGameServer pServer) : base(pServer) { }
        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            if(pMessage is ChatMessage) { handleChatMessage(pMessage as ChatMessage, pSender); }
            if(pMessage is ReqJoinRoom) { handleRoomRequest(pMessage as ReqJoinRoom, pSender); }
        }

        //handles room requests, if they want to leave or go into the game
        private void handleRoomRequest(ReqJoinRoom pRoomRequest, TCPMessageChannel pSender)
        {
            Logging.LogInfo("\nGot a room request with number : " + (int)pRoomRequest.room, Logging.debugState.DETAILED);
            try
            {
                //go back into login room
                if ((int)pRoomRequest.room == 0)
                {
                    Logging.LogInfo("Moving client to login room", Logging.debugState.DETAILED);
                    ConfJoinRoom confirmLoginRoom = new ConfJoinRoom();
                    confirmLoginRoom.room = 0;
                    pSender.SendMessage(confirmLoginRoom);
                    _server.availableRooms["Login"].AddMember(pSender);
                    removeAndCloseMember(pSender);
                    Console.WriteLine("count: " + _users.Count);
                }
                //go into game room
                else if ((int)pRoomRequest.room == 2)
                {
                    //needs at least 2 players
                    if (_users.Count >= 1)
                    {
                        Logging.LogInfo("Trying to move to game room", Logging.debugState.DETAILED);
                        ConfJoinRoom confirmGameRoom = new ConfJoinRoom();
                        confirmGameRoom.room = ConfJoinRoom.Rooms.GAME;
                        List<TCPMessageChannel> clientsToBeMoved = new List<TCPMessageChannel>();
                        //double foreach loop, because we cant adjust the list while looping through it, so we create new list with all that needs to be moved
                        //then we move them and remove them from the lobby, and thus the list.
                        foreach (TCPMessageChannel client in _users)
                        {
                            clientsToBeMoved.Add(client);
                        }
                        foreach(TCPMessageChannel client in clientsToBeMoved)
                        {
                            client.SendMessage(confirmGameRoom);
                            _server.availableRooms["Test0"].AddMember(client);
                            sendConfPlayer(client);
                            RemoveMember(client);
                        }
                    }
                    else
                    {
                        Logging.LogInfo("Not enough players are in lobby room to go to game room", Logging.debugState.DETAILED);
                    }
                }
                else
                {
                    Logging.LogInfo("Trying to get to a room that cant be handle in room request within lobby room", Logging.debugState.DETAILED);
                }
            }
            catch
            {
                Logging.LogInfo("Something went wrong in handle room request within lobby room, probably a null room", Logging.debugState.DETAILED);
            }
        }

        private void handleChatMessage(ChatMessage pMessage, TCPMessageChannel pSender)
        {
            ChatMessage newMessage = new ChatMessage();
            newMessage.textMessage = "[" + _server.allConnectedUsers[pSender].GetPlayerName() + "] : " + pMessage.textMessage;
            sendToAll(newMessage);
        }

        public override void AddMember(TCPMessageChannel pChannel)
        {
            Logging.LogInfo("User joined lobby room", Logging.debugState.DETAILED);
            base.AddMember(pChannel);
            ChatMessage newMessage = new ChatMessage();
            newMessage.textMessage = _server.allConnectedUsers[pChannel].GetPlayerName() + " has just joined the lobby!";
            sendToAll(newMessage);
        }

        public override void RemoveMember(TCPMessageChannel pChannel)
        {
            Logging.LogInfo("User left lobby room", Logging.debugState.DETAILED);
            base.RemoveMember(pChannel);
            ChatMessage newMessage = new ChatMessage();
            newMessage.textMessage = _server.allConnectedUsers[pChannel].GetPlayerName() + " left the lobby!";
            sendToAll(newMessage);
        }



        

    }

    
}
