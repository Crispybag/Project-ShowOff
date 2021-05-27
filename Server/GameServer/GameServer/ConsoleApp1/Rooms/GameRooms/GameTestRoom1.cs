using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class GameTestRoom1 : GameRoom
    {
        public GameTestRoom1(TCPGameServer pServer) : base(pServer, 0, 0, 0)
        {
            //GenerateGridFromText("../../testLevel1.txt");
        }
    }
}
