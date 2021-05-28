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

        public Box(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pRoom, CollInteractType.SHOVE)
        {
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            ID = pID;
            room = pRoom;
            room.roomArray[position[0], position[1], position[2]].Add(7);
            objectIndex = 7;
        }
    }
}
