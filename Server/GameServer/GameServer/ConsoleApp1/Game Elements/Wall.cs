using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    class Wall : GameObject
    {
        GameRoom _room;
        public Wall(GameRoom pRoom, int pX, int pY) : base(CollInteractType.SOLID)
        {
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]] = 2;
        }
    }
}
