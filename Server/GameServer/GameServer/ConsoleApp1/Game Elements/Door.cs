using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Door : GameObject
    {
        GameRoom _room;

        public Door(GameRoom pRoom, int pX, int pY) : base(pRoom, CollInteractType.SOLID)
        {
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(7);
        }

        public void CheckDoor()
        {

        }

    }
}
