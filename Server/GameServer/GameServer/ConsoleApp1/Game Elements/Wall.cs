using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    public class Wall : GameObject
    {
        GameRoom _room;
        public Wall(GameRoom pRoom, int pX, int pY, int pZ) : base(pRoom, CollInteractType.SOLID)
        {
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            _room.roomArray[position[0], position[1], position[2]].Add(2);
            objectIndex = 2;
        }
    }
}
