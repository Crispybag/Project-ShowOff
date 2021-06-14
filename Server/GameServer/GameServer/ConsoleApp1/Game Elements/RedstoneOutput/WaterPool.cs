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
                if (direction == 0)
                {
                    oldPos = currentPos;
                    currentPos++;
                    if (currentPos > waterLevelPositions.Count-1)
                    {
                        currentPos = waterLevelPositions.Count-1;
                    }
                }
                if (direction == 1)
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

                    Box coolBox = null;
                    if (room.OnCoordinatesGetGameObject(water.x(), water.y() + 1, water.z(), 7) is Box)
                    {
                        coolBox = room.OnCoordinatesGetGameObject(water.x(), water.y() + 1, water.z(), 7) as Box;
                    }

                    water.MoveDirection(waterLevelPositions[currentPos].x() - waterLevelPositions[oldPos].x(), waterLevelPositions[currentPos].y() - waterLevelPositions[oldPos].y(), waterLevelPositions[currentPos].z() - waterLevelPositions[oldPos].z());
                    
                    if (coolBox != null)
                    {
                        coolBox.MovePosition(x(), y() + 1, z());
                        coolBox.sendBoxPackage(false);
                    }
                }

                ConfWaterPool waterPool = new ConfWaterPool();
                waterPool.ID = ID;
                waterPool.x = waterLevelPositions[currentPos].x();
                waterPool.y = waterLevelPositions[currentPos].y();
                waterPool.z = waterLevelPositions[currentPos].z();

                room.sendToAll(waterPool);
            }
            catch (Exception e)
            {
                Logging.LogInfo("\nI AM CRYING " + waterLevelPositions.Count);
                Console.WriteLine("currentPos = " + currentPos);
                Console.WriteLine("oldPos = " + oldPos);
                Logging.LogInfo(e.Message);
            }
        }
    }
}
