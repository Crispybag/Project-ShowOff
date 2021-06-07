using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    public class Water : GameObject
    {
        public Water(GameRoom pRoom, int pX, int pY, int pZ) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            room = pRoom;
            //room.PrintGrid(room.roomArray);
            room.roomArray[x(), y(), z()].Add(this);

            objectIndex = 16;
        }
    }
}
