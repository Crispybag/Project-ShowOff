using sharedAngy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    abstract class GameRoom : Room
    {
        public int[,] roomArray;

        public GameRoom(TCPGameServer pServer, int roomWidth, int roomHeight) : base(pServer)
        {
            roomArray = new int[roomWidth, roomHeight];
        }


        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            if (pMessage is ReqKeyDown)
            {

            }

            if (pMessage is ReqKeyUp)
            {

            }
        }

        private void handleReqKeyDown(ReqKeyDown pKeyDown)
        {

        }

        private void handleReqKeyUp()
        {

        }
    }
}
