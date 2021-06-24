using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{

    /// <summary>
    /// (Leo) Implements inwalking tiles
    /// </summary>
    public class Wall : GameObject
    {
        public Wall(GameRoom pRoom, int pX, int pY, int pZ) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            room = pRoom;
            Logging.LogInfo(x() + " " + y() + " " + z());
            //room.PrintGrid(room.roomArray);
            room.roomArray[x(), y(), z()].Add(this);

            objectIndex = 2;
        }
    }
}
