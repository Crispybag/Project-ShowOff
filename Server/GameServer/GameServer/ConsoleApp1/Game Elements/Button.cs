using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Button : Actuator
    {
        public Button(GameRoom pRoom, int pX, int pY, int pID) : base(pRoom, pX, pY, pID)
        {
            _room = pRoom;
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(4);
            objectIndex = 4;
        }
    }
}

