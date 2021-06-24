using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    class WaterPool : GameObject
    {
        public List<GameObject> waterBlocks = new List<GameObject>();
        public List<GameObject> waterLevelPositions = new List<GameObject>();
        public int currentPos;
        public int oldPos;
        public int ID;

        public WaterPool(GameRoom pRoom, int pX, int pY, int pZ, int pID, CollInteractType pMoveState) : base(pX, pY, pZ, pRoom, pMoveState)
        {
            room.roomArray[x(), y(), z()].Add(this);
            ID = pID;
        }

        public void moveWater(int direction)
        {
            try
            {
                if (direction == 1)
                {
                    oldPos = currentPos;
                    currentPos++;
                    if (currentPos > waterLevelPositions.Count-1)
                    {
                        currentPos = waterLevelPositions.Count-1;
                    }
                }
                if (direction == 0)
                {
                    oldPos = currentPos;
                    currentPos--;
                    if (currentPos < 0)
                    {
                        currentPos = 0;

                    }
                }
                foreach (GameObject water in waterBlocks)
                {
                    water.MoveDirection(waterLevelPositions[currentPos].x() - waterLevelPositions[oldPos].x(), waterLevelPositions[currentPos].y() - waterLevelPositions[oldPos].y(), waterLevelPositions[currentPos].z() - waterLevelPositions[oldPos].z());
                }
            }
            catch (Exception e)
            {
                Logging.LogInfo("I AM CRYING" + waterLevelPositions.Count);
                Logging.LogInfo(e.Message);
            }
        }
    }
}
