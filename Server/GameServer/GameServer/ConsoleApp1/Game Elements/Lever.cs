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
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            isActivated = activated;
            _room.roomArray[position[0], position[1]].Add(4);
            objectIndex = 4;
        }
    }
}
