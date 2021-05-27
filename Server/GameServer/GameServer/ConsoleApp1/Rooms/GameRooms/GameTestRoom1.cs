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
            //generateGridFromText("../../testLevel1.txt");
        }
    }
}
