using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{

    /// <summary>
    /// (Leo) Implements inwalking tiles
    /// </summary>
    public class AirChannel : GameObject
    {
        public int ID;
        private bool isOn = true;
        public List<int> actuators = new List<int>();
        public int[] direction = new int[3];
        public AirChannel(GameRoom pRoom, int pX, int pY, int pZ, int pDirX, int pDirY, int pDirZ, int pID) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            direction[0] = pDirX;
            direction[1] = pDirY;
            direction[2] = pDirZ;
            objectIndex = 13;
        }
        /// <summary>
        /// (Leo) checks if player can be pushed on a certain tile
        /// </summary>
        /// <param name="pPosition"></param>
        /// <returns></returns>
        public bool CanPushPlayer(int[] pPosition)
        {
            try
            {
                Logging.LogInfo(pPosition[0] + " " + pPosition[1] + " " + pPosition[2]);

                if (room.OnCoordinatesCanMove(pPosition[0] + direction[0], pPosition[1] + direction[1], pPosition[2] + direction[2])) return true;
                else return false;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// push the player to new location
        /// </summary>
        /// <param name="pPosition"></param>
        /// <returns></returns>
        public int[] PushPlayer(int[] pPosition)
        {
            try
            {
                int[] newPosition = pPosition;
                if (isOn)
                {
                    newPosition[0] += direction[0];
                    newPosition[1] += direction[1];
                    newPosition[2] += direction[2];
                }
                return newPosition;

            }
            catch
            {
                Logging.LogInfo("Something messed up in PushPlayer", Logging.debugState.DETAILED);
                return pPosition;
            }
        }

        /// <summary>
        /// (Leo) handles the toggling of the air channel
        /// </summary>
        public void ToggleAirChannel()
        {
            if (!isOn)
            {
                SetState(CollInteractType.PASS);
            }
            else
            {
                SetState(CollInteractType.SOLID);
            }

        }

        public void CheckAirChannel()
        {
            isOn = checkActuators();
            ToggleAirChannel();
        }

        private bool checkActuators()
        {
            foreach (int actuator in actuators)
            {
                try
                {
                    if (!(room.InteractableGameobjects[actuator] as Actuator).isActivated)
                    {
                        return false;
                    }
                }
                catch
                {
                    Logging.LogInfo("Door.cs: Could not handle actuator, probably not in list in room!", Logging.debugState.DETAILED);
                }
            }
            return true;
        }

    }
}
