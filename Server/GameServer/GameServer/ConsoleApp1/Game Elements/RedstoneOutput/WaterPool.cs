using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class WaterPool : GameObject
    {
        public List<GameObject> waterBlocks = new List<GameObject>();
        public List<GameObject> waterLevelPositions = new List<GameObject>();
        public int currentPos;
        public int ID;

        public WaterPool(GameRoom pRoom, int pX, int pY, int pZ, int pID, CollInteractType pMoveState) : base(pX, pY, pZ, pRoom, pMoveState)
        {
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            ID = pID;
        }

        public void moveWater(int direction)
        {
            if(direction == 1)
            {
                currentPos++;
                if(currentPos > waterLevelPositions.Count)
                {
                    currentPos = waterLevelPositions.Count;
                }
            }
            if (direction == 0)
            {
                currentPos--;
                if(currentPos < 0)
                {
                    currentPos = 0;
                }
            }
            foreach(GameObject water in waterBlocks)
            {
                water.MovePosition(waterLevelPositions[currentPos].x(), waterLevelPositions[currentPos].y(), waterLevelPositions[currentPos].z());
            }
        }
    }
}
