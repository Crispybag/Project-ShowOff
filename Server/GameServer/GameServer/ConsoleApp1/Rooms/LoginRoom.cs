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
            Console.WriteLine("User Joined test room");
            base.AddMember(pChannel);
        }

        private void handleReqJoinServer(ReqJoinServer reqJoinServer, TCPMessageChannel pSender)
        {

            ConfJoinServer joinMessage = new ConfJoinServer();
            foreach (PlayerInfo pInfo in _server.allConnectedUsers.Values)
            {

                if (reqJoinServer.requestedName == pInfo.GetPlayerName())
                {
                    joinMessage.acceptStatus = false;
                    joinMessage.message = "A user with the same name already joined the server, please choose a different name.";
                    pSender.SendMessage(joinMessage);
                    removeAndCloseMember(pSender);
                    return;
                }
                
            }
            joinMessage.acceptStatus = true;
            joinMessage.message = "Client accepted"; 
            pSender.SendMessage(joinMessage);
            _server.AddPlayerInfo(pSender, reqJoinServer.requestedName);
            
        }
    }
}
