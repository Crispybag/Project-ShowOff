using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Lever : Actuator
    {
        public Lever(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated) : base(pRoom, pX, pY, pZ, pID, pActivated)
        {
            ID = pID;
            room = pRoom;
            isActivated = pActivated;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 4;
        }
    }
}
