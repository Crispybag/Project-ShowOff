using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class TestRoom : Room
    {
        public TestRoom(TCPGameServer pServer) : base(pServer) { }
        private int _counter;
        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            if (pMessage is ReqAddCount)
            {
                Console.WriteLine("Received a Request Add Count");
                handleReqAddCount();
            }
            if (pMessage is ReqMove)
            {
                handleReqMove(pMessage as ReqMove);
            }
            if (pMessage is ReqJoinServer)
            {
                handleReqJoinServer(pMessage as ReqJoinServer, pSender);
            }
        }
        public void AddMember(TCPMessageChannel pChannel)
        {
            Console.WriteLine("User Joined test room");
            addMember(pChannel);
        }

        private void handleReqAddCount()
        {
            _counter++;
            ConfAddCount addCount = new ConfAddCount();
            addCount.totalCount = _counter;
            sendToAll(addCount);
        }

        private void handleReqMove(ReqMove pMove)
        {
            ConfMove confMove = new ConfMove();
            confMove.dirX = pMove.dirX;
            confMove.dirY = pMove.dirY;
            confMove.dirZ = pMove.dirZ;
            sendToAll(confMove);

        }

        private void handleReqJoinServer(ReqJoinServer reqJoinServer, TCPMessageChannel pSender)
        {

            ConfJoinServer joinMessage = new ConfJoinServer();
            foreach (PlayerInfo pInfo in _server._allConnectedUsers.Values)
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
    }
}
