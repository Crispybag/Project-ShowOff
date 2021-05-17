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
            confMove.oldX = pMove.oldX;
            confMove.oldY = pMove.oldY;
            confMove.oldZ = pMove.oldZ;
            sendToAll(confMove);

        }
    }
}
