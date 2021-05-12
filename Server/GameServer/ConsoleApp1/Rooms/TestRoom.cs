using System;
using System.Collections.Generic;
using System.Text;
using Shared;

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
    }
}
