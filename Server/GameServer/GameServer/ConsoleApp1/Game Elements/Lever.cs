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
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            room = pRoom;
            isActivated = pActivated;
            room.roomArray[position[0], position[1], position[2]].Add(4);
            objectIndex = 4;
        }
    }
}
