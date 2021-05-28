using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class EmptyGameObject : GameObject
    {
        //Class for if you just need points in the scene, like for the elevator
        public EmptyGameObject(GameRoom pRoom, int pX, int pY, int pZ) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            room = pRoom;
        }
    }
}