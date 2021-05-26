using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Lever : Actuator
    {
        public Lever(GameRoom pRoom, int pX, int pY, int pID, bool pActivated) : base(pRoom, pX, pY, pID, pActivated)
        {
            _room = pRoom;
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            isActivated = pActivated;
            position[2] = pY;
            isActivated = activated;
            _room.roomArray[position[0], position[1], position[2]].Add(4);
            objectIndex = 4;
        }
    }
}
