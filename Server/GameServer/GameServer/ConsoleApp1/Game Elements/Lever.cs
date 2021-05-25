using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Lever : Actuator
    {
        public Lever(GameRoom pRoom, int pX, int pY, int _ID, bool activated) : base(pRoom, pX, pY, activated)
        {

        }
    }
}
