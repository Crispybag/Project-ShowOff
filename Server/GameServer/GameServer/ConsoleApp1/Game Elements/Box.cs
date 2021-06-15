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
        private bool containsWater;
        private bool hasFallen = false;
        public int ID;

        public Box(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 7;
        }
        public void CheckGrounded()
        {
            try
            {
                bool isGrounded = false;

                while (!isGrounded)
                {
                    int[] pPosition = new int[3] { x(), y() - 1, z() };
                    //if object is 1 below you, stop falling
                    if (isBlockedByObject(pPosition) || y() <= 0)
                    {
                        isGrounded = true;
                    }

                    //else fall 1 further down
                    else
                    {
                        MoveDirection(0, -1, 0);
                        hasFallen = true;
                    }
                }
            }
            catch
            {

            }
        }

        private bool isBlockedByObject(int[] position)
        {
            List<GameObject> gameObjects = room.OnCoordinatesGetGameObjects(position);
            foreach (GameObject gameObject in gameObjects)
            {
                //Console.WriteLine(gameObject.objectIndex);
                if (gameObject.collState == CollInteractType.SOLID) return true;
            }
            return false;
        }

        public void sendBoxPackage(bool pIsPickedUp)
        {
            try
            {
                if (pIsPickedUp)
                {
                    SetState(CollInteractType.PASS);
                }
                else
                {
                    SetState(CollInteractType.SOLID);
                }
                BoxInfo boxInfo = new BoxInfo();
                boxInfo.ID = ID;
                boxInfo.isPickedUp = pIsPickedUp;
                boxInfo.posX = x() + room.minX;
                boxInfo.posY = y() + room.minY;
                boxInfo.posZ = z() + room.minZ;
                room.sendToAll(boxInfo);
            }
            catch
            {
                Logging.LogInfo("Something went wrong when trying to adjust the box", Logging.debugState.SIMPLE);
            }
        }

        public bool tileContainsWater(int pID)
        {
            List<GameObject> objectsOnCoord = room.OnCoordinatesGetGameObjects(x(), y(), z() );
            int i = 0;
            containsWater = false;
            foreach (GameObject index in objectsOnCoord)
            {
                i++;
                if(index.collState == CollInteractType.SOLID && index.objectIndex != 16 && index.objectIndex != 7 && pID != this.ID)
                {
                    return false;
                }
                else if(index.objectIndex == 16)
                {
                    containsWater = true;
                }
                if(i == objectsOnCoord.Count && containsWater)
                {
                    return true;
                }
            }
            return false;
        }


        public override void Update()
        {
            base.Update();
            CheckGrounded();
            handleAirChannelHit(new int[3] { x(), y(), z() });

            if (hasFallen) { sendBoxPackage(false); hasFallen = false; }
        }

        int infLoopPrevention;

        /// <summary>
        /// (Leo) Interaction with box and airchannels (mostly copied over from player)
        /// </summary>
        /// <param name="pPosition"></param>
        private void handleAirChannelHit(int[] pPosition)
        {
            infLoopPrevention++;
            if (room.OnCoordinatesGetGameObject(pPosition, 13) is AirChannel && infLoopPrevention < 20)
            {
                AirChannel airChannel = room.OnCoordinatesGetGameObject(pPosition, 13) as AirChannel;

                if (airChannel.CanPushPlayer(pPosition))
                {
                    MovePosition(airChannel.PushPlayer(pPosition));
                    sendBoxPackage(false);
                }

                else
                {
                    handleAirChannelHit(airChannel.PushPlayer(pPosition));
                }
            }
            infLoopPrevention = 0;
        }
    }
}
