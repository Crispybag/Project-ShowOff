using sharedAngy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    abstract class GameRoom : Room
    {
        public int[,] roomArray;
        //quick cheat sheet
        //0 is empty
        //1 is player
        //2 is wall

        public List<Player> players;
 
        public GameRoom(TCPGameServer pServer, int roomWidth, int roomHeight) : base(pServer)
        {
            roomArray = new int[roomWidth, roomHeight];
            players = new List<Player>();
        }


        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a package! Trying to handle", Logging.debugState.SPAM);
            if (pMessage is ReqKeyDown)
            {
                handleReqKeyDown(pMessage as ReqKeyDown, pSender);
            }

            if (pMessage is ReqKeyUp)
            {
                handleReqKeyUp(pMessage as ReqKeyUp, pSender);
            }
        }

        private void handleReqKeyDown(ReqKeyDown pKeyDown, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a HandleReqKeyDown Package", Logging.debugState.DETAILED);
            foreach (Player player in players)
            {
                if (pSender == player.getClient())
                {
                    player.checkInput(pKeyDown.keyInput);
                }
            }
        }

        private void handleReqKeyUp(ReqKeyUp pKeyUp, TCPMessageChannel pSender)
        {
            //dont forget to make one here later
        }

        public override void AddMember(TCPMessageChannel pListener)
        {
            Logging.LogInfo("Added client to gameRoom0");
            base.AddMember(pListener);
            players.Add(new Player(this, pListener));
        }

    }
}
