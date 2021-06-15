using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{

    /// <summary>
    /// (Leo) Implements inwalking tiles
    /// </summary>
    public class AirChannel : RedstoneOutput
    {
        public int[] direction = new int[3];
        public AirChannel(GameRoom pRoom, int pX, int pY, int pZ, int pDirX, int pDirY, int pDirZ, int pID, List<int> pActuators) : base(pRoom, pX, pY, pZ, pID, CollInteractType.SOLID, pActuators)
        {
            room.roomArray[x(), y(), z()].Add(this);
            direction[0] = pDirX;
            direction[1] = pDirY;
            direction[2] = pDirZ;
            objectIndex = 13;

            ToggleAirChannel();
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
                if (isActivated)
                {
                    int xTest = newPosition[0];
                    int yTest = newPosition[1];
                    int zTest = newPosition[2];
                    int xDir = direction[0];
                    int yDir = direction[1];
                    int zDir = direction[2];


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
            if (isActivated)
            {
                SetState(CollInteractType.SOLID);
            }
            else
            {
                SetState(CollInteractType.PASS);
            }

        }

        public override void CheckOutput()
        {
            base.CheckOutput();
            ToggleAirChannel();
        }
    }
}
