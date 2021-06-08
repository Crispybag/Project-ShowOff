using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class LevelLoader : GameObject
    {
        public string fileName;
        public LevelLoader(GameRoom pRoom, int pX, int pY, int pZ, string pFileName) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            room = pRoom;
            fileName = "../../../../LevelFiles/" + pFileName;
            //room.PrintGrid(room.roomArray);
            room.roomArray[x(), y(), z()].Add(this);

            objectIndex = 14;
        }
    }
}

