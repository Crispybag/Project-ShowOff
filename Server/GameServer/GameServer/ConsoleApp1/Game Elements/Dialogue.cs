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

        public Dialogue(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pX, pY, pZ, pRoom, CollInteractType.PASS)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 15;
        }
    }
}
