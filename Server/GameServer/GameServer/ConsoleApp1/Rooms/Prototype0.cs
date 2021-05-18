using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Prototype0 : Room
    {
        public Prototype0(TCPGameServer pServer) : base(pServer) 
        {
        }

        #region Server startup
        public void AddMember(TCPMessageChannel pChannel)
        {
            Console.WriteLine("User Joined test room");
            AddMember(pChannel);
        }

        private void handleReqJoinServer(ReqJoinServer reqJoinServer, TCPMessageChannel pSender)
        {

            ConfJoinServer joinMessage = new ConfJoinServer();
            foreach (PlayerInfo pInfo in _server.allConnectedUsers.Values)
            {

                if (reqJoinServer.requestedName == pInfo.GetPlayerName())
                {
                    joinMessage.acceptStatus = false;
                    pSender.SendMessage(joinMessage);
                    removeAndCloseMember(pSender);
                    return;
                }

            }
            joinMessage.acceptStatus = true;
            pSender.SendMessage(joinMessage);
            _server.AddPlayerInfo(pSender, reqJoinServer.requestedName);

        }

        #endregion

        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            try
            {
                if (pMessage is ReqJoinServer)
                {
                    handleReqJoinServer(pMessage as ReqJoinServer, pSender);
                }
                if (pMessage is ReqMove)
                {
                    handleReqMove(pMessage as ReqMove);
                }
            }
            catch
            {
                Console.WriteLine("A package has been send but we dont know how to handle this!");
            }

        }

        private void handleReqMove(ReqMove pMove)
        {
            ConfMove confMove = new ConfMove();
            confMove.name = pMove.name;
            confMove.dirX = pMove.dirX;
            confMove.dirY = pMove.dirY;
            confMove.dirZ = pMove.dirZ;
            sendToAll(confMove);
        }


    }
}
