using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Lever : GameObject
    {
        GameRoom _room;
        bool isActivated = false;
        public Lever(GameRoom pRoom, int pX, int pY, bool activated) : base(CollInteractType.SOLID)
        {
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            isActivated = activated;
            _room.roomArray[position[0], position[1]] = 4;
        }
    }
}
