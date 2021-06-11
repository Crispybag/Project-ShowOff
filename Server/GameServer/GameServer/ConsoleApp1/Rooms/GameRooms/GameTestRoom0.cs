using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class GameTestRoom0 : GameRoom
    {
        public GameTestRoom0(TCPGameServer pServer, int pWidth, int pHeight, int pLength) : base(pServer, pWidth, pHeight, pLength)
        {

            GenerateGridFromText("../../../../LevelFiles/Features.txt");
            //CopyGrid(roomStatic, roomArray);
            //PrintGrid(roomArray);
        }

        public override void AddMember(TCPMessageChannel pListener)
        {
            base.AddMember(pListener);
            SpawnPlayer(pListener, _server.allConnectedUsers[pListener].GetPlayerIndex());
            //PrintGrid(roomArray);
        }
    }
}
