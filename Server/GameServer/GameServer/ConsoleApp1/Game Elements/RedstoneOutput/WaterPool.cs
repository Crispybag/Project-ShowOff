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
        public bool isGoingUp = true;
        public WaterPool(GameRoom pRoom, int pX, int pY, int pZ, int pID, CollInteractType pMoveState) : base(pX, pY, pZ, pRoom, pMoveState)
        {
            room.roomArray[x(), y(), z()].Add(this);
            ID = pID;
        }

        public void moveWater()
        {
            try
            {
                oldPos = currentPos;

                if (currentPos >= waterLevelPositions.Count - 1)
                {
                    isGoingUp = false;
                }
                else if (currentPos <= 0)
                {
                    isGoingUp = true;
                }

                if (!isGoingUp)
                {
                    currentPos--;
                }
                else if (isGoingUp)
                {
                    currentPos++;
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
                        coolBox.MoveDirection(waterLevelPositions[currentPos].x() - waterLevelPositions[oldPos].x(), waterLevelPositions[currentPos].y() - waterLevelPositions[oldPos].y(), waterLevelPositions[currentPos].z() - waterLevelPositions[oldPos].z());
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
            }
        }
    }
}
