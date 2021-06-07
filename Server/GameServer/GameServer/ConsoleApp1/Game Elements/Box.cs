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
                Console.WriteLine(gameObject.objectIndex);
                if (gameObject.collState == CollInteractType.SOLID) return true;
            }
            return false;
        }

        public void SendBoxPackage(GameObject box, int[] position, bool pIsPickedUp)
        {
            try
            {
                if (pIsPickedUp)
                {
                    box.SetState(CollInteractType.PASS);
                }
                else
                {
                    box.SetState(CollInteractType.SOLID);
                }
                BoxInfo boxInf = new BoxInfo();
                boxInf.ID = (box as Box).ID;
                boxInf.isPickedUp = pIsPickedUp;
                boxInf.posX = position[0] + room.minX;
                boxInf.posY = position[1] + room.minY;
                boxInf.posZ = position[2] + room.minZ;
                room.sendToAll(boxInf);
            }
            catch
            {
                Logging.LogInfo("Something went wrong when trying to adjust the box", Logging.debugState.SIMPLE);
            }
        }

        public void SendBoxPackage(GameObject box, int pX, int pY, int pZ, bool pIsPickedUp)
        {
            try
            {
                BoxInfo boxInf = new BoxInfo();
                boxInf.ID = (box as Box).ID;
                boxInf.isPickedUp = pIsPickedUp;
                boxInf.posX = pX + room.minX;
                boxInf.posY = pY + room.minY;
                boxInf.posZ = pZ + room.minZ;
                room.sendToAll(boxInf);
            }
            catch
            {
                Logging.LogInfo("Something went wrong when trying to adjust the box", Logging.debugState.SIMPLE);
            }
        }

    }
}
