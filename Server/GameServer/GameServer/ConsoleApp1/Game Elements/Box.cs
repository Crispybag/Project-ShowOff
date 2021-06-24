using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    /// <summary>
    /// (Leo) Contains the information about the box
    /// </summary>
    public class Box : GameObject
    {

        public int ID;

        public Box(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 7;
        }
    }
}
