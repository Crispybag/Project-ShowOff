using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    public class Box : GameObject
    {
        GameRoom _room;
        public Box(GameRoom pRoom, int pX, int pY) : base(pRoom, CollInteractType.SHOVE)
        {
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(2);
        }

        public bool CanBeShoved(int pX, int pY)
        {
            if (!_room.coordinatesContain(pX, pY, 5))
            {

            }
        }
    }
}
