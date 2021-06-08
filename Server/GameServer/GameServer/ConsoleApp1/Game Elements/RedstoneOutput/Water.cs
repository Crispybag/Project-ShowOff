using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    public class Water : GameObject
    {
        public bool isOn;

        public Water(GameRoom pRoom, int pX, int pY, int pZ) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            room = pRoom;
            //room.PrintGrid(room.roomArray);
            room.roomArray[x(), y(), z()].Add(this);

            objectIndex = 16;
        }

        /// <summary>
        /// (Leo) checks if player can be pushed on a certain tile
        /// </summary>
        /// <param name="pPosition"></param>
        /// <returns></returns>
        public bool CanPushBox(int[] pPosition)
        {
            try
            {

                if (room.OnCoordinatesCanMove(pPosition[0], pPosition[1] +1, pPosition[2])) return true;
                else return false;
            }
            catch
            {
                return false;
            }

        }

        public bool CanPushBox(int pX, int pY, int pZ)
        {
            try
            {

                if (CanMoveUp(pX, pY + 1, pZ)) return true;
                else return false;
            }
            catch
            {
                return false;
            }

        }

        private bool CanMoveUp(int pX, int pY, int pZ)
        {
            if (room.roomArray[pX, pY, pZ].Count != 0)
            {
                foreach (GameObject obj in room.roomArray[pX, pY, pZ])
                {
                    if (obj.collState == GameObject.CollInteractType.SOLID && obj.objectIndex != 16)
                        return false;
                }
            }
            return true;
        }


/// <summary>
/// push the player to new location
/// </summary>
/// <param name="pPosition"></param>
/// <returns></returns>
public int[] PushBox(int[] pPosition)
        {
            try
            {
                int[] newPosition = pPosition;
                if (isOn)
                {
                    newPosition[1] += 1;
                }
                return newPosition;

            }
            catch
            {
                Logging.LogInfo("Something messed up in PushBox", Logging.debugState.DETAILED);
                return pPosition;
            }
        }

        public int[] PushBox(int pX, int pY, int pZ)
        {
            try
            {
                int[] newPosition = new int[3];
                newPosition[0] = pX;
                newPosition[1] = pY;
                newPosition[2] = pZ;
                Console.WriteLine("Our current position of the box is : "+ pX + ","+ pY +"," + pZ);
                //if (isOn)
                //{
                    newPosition[1] += 1;
                //}
                Console.WriteLine("the next position of the box is : " + newPosition[0] + "," + newPosition[1] + "," + newPosition[2]);
                return newPosition;

            }
            catch
            {
                Logging.LogInfo("Something messed up in PushBox", Logging.debugState.DETAILED);
                int[] newPosition = new int[3];
                newPosition[0] = pX;
                newPosition[1] = pY;
                newPosition[2] = pZ;
                return newPosition;
            }
        }

    }
}
