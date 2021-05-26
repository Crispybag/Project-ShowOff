using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    public class Box : GameObject
    {
        GameRoom _room;
        public Box(GameRoom pRoom, int pX, int pY, int pZ) : base(pRoom, CollInteractType.SHOVE)
        {
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            _room.roomArray[position[0], position[1], position[2]].Add(7);
            objectIndex = 7;
        }
    }
}
