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

        public WaterPool(GameRoom pRoom, int pX, int pY, int pZ, CollInteractType pMoveState) : base(pX, pY, pZ, pRoom, pMoveState)
        {
            room = pRoom;
            //room.PrintGrid(room.roomArray);
            room.roomArray[x(), y(), z()].Add(this);
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
