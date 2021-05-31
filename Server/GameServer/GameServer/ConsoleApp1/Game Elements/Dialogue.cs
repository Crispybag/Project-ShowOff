using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Sub class of actuator, because it has very similar functionalities to actuator, after activated once, it has been fixed.
    /// </summary>
    class Dialogue : GameObject
    {

        public int ID;

        public Dialogue(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pRoom, CollInteractType.PASS)
        {
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            room = pRoom;
            room.roomArray[position[0], position[1], position[2]].Add(15);
            objectIndex = 15;
        }
    }
}
