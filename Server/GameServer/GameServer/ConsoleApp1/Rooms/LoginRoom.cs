using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class LoginRoom : Room
    {
        public LoginRoom(TCPGameServer pServer) : base(pServer) { }
        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            if (pMessage is ReqJoinServer)
            {
                handleReqJoinServer(pMessage as ReqJoinServer, pSender);
            }
        }

        public void AddMember(TCPMessageChannel pChannel)
        {
            Console.WriteLine("User joined login room");
            addMember(pChannel);
        }

        private void handleReqJoinServer(ReqJoinServer reqJoinServer, TCPMessageChannel pSender)
        {
            ConfJoinServer joinMessage = new ConfJoinServer();
            if(reqJoinServer.requestedName.Length < 3)
            {
                joinMessage.acceptStatus = false;
                joinMessage.message = "Please use a longer name";
                Logging.LogInfo("Client got rejected with a too short name:  " + reqJoinServer.requestedName, Logging.debugState.DETAILED);
                pSender.SendMessage(joinMessage);
                removeAndCloseMember(pSender);
                return;
            }
            foreach (PlayerInfo pInfo in _server.allConnectedUsers.Values)
            {
                if (reqJoinServer.requestedName == pInfo.GetPlayerName())
                {
                    joinMessage.acceptStatus = false;
                    joinMessage.message = "A user with the same name already joined the server, please choose a different name.";
                    Logging.LogInfo("Client got rejected with the duplicate name:  " + reqJoinServer.requestedName, Logging.debugState.DETAILED);
                    pSender.SendMessage(joinMessage);
                    removeAndCloseMember(pSender);
                    return;
                }
                
            }
            joinMessage.acceptStatus = true;
            joinMessage.message = "Client accepted with the name" + reqJoinServer.requestedName;
            Logging.LogInfo("Client got accepted with the name:  " + reqJoinServer.requestedName + "\n", Logging.debugState.DETAILED);
            pSender.SendMessage(joinMessage);
            _server.AddPlayerInfo(pSender, reqJoinServer.requestedName);
            _server.availableRooms["Lobby"].addMember(pSender);
            removeMember(pSender);
        }
    }
}
