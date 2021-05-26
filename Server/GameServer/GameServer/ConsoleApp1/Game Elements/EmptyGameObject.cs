using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class EmptyGameObject : GameObject
    {
        GameRoom _room;

        public EmptyGameObject(GameRoom pRoom, int pX, int pY) : base(pRoom, CollInteractType.SOLID)
        {
            position[0] = pX;
            position[1] = pY;
        }
    }
}