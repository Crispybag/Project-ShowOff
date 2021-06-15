using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    /// <summary>
    /// (Leo) This class implements a slope, which lets the player move up and down.
    /// </summary>
    public class Slope : GameObject
    {

        //rotation of the slope
        private int[] orientation;
        
        //since slopes are 2 * 1 * 1 it takes up 3 positions

        //slope positions
        //s2     
        //s1 s0  

        private int[] _s0Position;
       // private int[] _s1Position;
        private int[] _s2Position;

        #region intialization
        public Slope(GameRoom pRoom, int pX, int pY, int pZ, int pRotation) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            room = pRoom;
            //set the orientation based on the y-rotation of the objects
            orientation = new int[2];
            switch (pRotation)
            {
                case (0):
                    orientation[0] = 0; orientation[1] = -1;
                    break;

                case (90):
                    orientation[0] = -1; orientation[1] = 0;
                    break;

                case (180):
                    orientation[0] = 0; orientation[1] = 1;
                    break;

                case (270):
                    orientation[0] = 1; orientation[1] = 0;
                    break;

                case (-90):
                    orientation[0] = 1; orientation[1] = 0;
                    break;

                default:
                    Logging.LogInfo("found a slope at a non 90 degree angle, bruh", Logging.debugState.SIMPLE);
                    break;

            }

            //set positions to respective parts
            try
            {
                _s0Position = new int[3] { x(), y(), z() };
                _s2Position = new int[3] { x() + orientation[0], y() + 1, z() + orientation[1] };                        

                //load objects in the scene
                objectIndex = 10;
                room.roomArray[_s0Position[0], _s0Position[1], _s0Position[2]].Add(this);
                room.roomArray[_s2Position[0], _s2Position[1], _s2Position[2]].Add(this);

                //place a wall to obstruct people walking under the slope
                Wall wall = new Wall(room, x() + orientation[0], y(), z() + orientation[1]);
                room.roomArray[x() + orientation[0], y(), z() + orientation[1]].Add(wall);
            }

            catch
            {
                Logging.LogInfo("Index falls out of range of the array when initialising slope, make sure to generate slopes properly and expand room if necessary", Logging.debugState.DETAILED);
            }

            
        }
        #endregion

        /// <summary>
        /// (Leo) Checks if the player can move on the slope or not based on the position it is trying to hit the slope and the orientation the player currently has
        /// </summary>
        /// <param name="pPosition"></param>
        /// <param name="pOrientation"></param>
        /// <returns></returns>
        public int CanMoveOnSlope(int[] pPosition, int[] pOrientation)
        {
            //if it hits position 2 and has opposing orientation
            if (PositionEquals(pPosition, _s2Position))
            {
                if (pOrientation[0] == -orientation[0] && orientation[1] == -pOrientation[1])
                {
                    try
                    {
                        //make sure that the room the player wants to move to is empty and exists
                        if (room.OnCoordinatesCanMove(OneInFront(_s0Position, new int[2] { -orientation[0], -orientation[1] }))) { return 0; }
                        return 1;

                    }
                    catch
                    {
                        Logging.LogInfo("Slope tries to send player outside of bounds of array in CanMoveOnSlope", Logging.debugState.DETAILED);
                        return 2;
                    }
                }
            }
            //if it hits position 0 and has same orientation
            else if (PositionEquals(pPosition, _s0Position) && pOrientation[0] == orientation[0] && pOrientation[1] == orientation[1])
            {
                try
                {
                    //make sure that the room the player wants to move to is empty and exists
                    if (room.OnCoordinatesCanMove(OneInFront(_s2Position, orientation))) { return 0; }
                    return 1;

                }
                catch
                {
                    Logging.LogInfo("Slope tries to send player outside of bounds of array in CanMoveOnSlope", Logging.debugState.DETAILED);
                    return 2;
                }
            }
            return 2;
        }

        /// <summary>
        /// (leo) Move the player on the slope based on which of the 2 positions the player has hit
        /// </summary>
        /// <param name="pPosition"></param>
        /// <returns></returns>
        public int[] MoveOnSlope(int[] pPosition)
        {
            try
            {
                //if position is s0, move it one past s2
                Logging.LogInfo("Coordinates are " + _s2Position[0] + orientation[0] + ", " + _s2Position[1] + ", " + _s2Position[2] + orientation[1], Logging.debugState.DETAILED);
                if (PositionEquals(pPosition, _s0Position)) { return OneInFront(_s2Position, orientation); }
                //if position is s2, move it one past s0
                else if (PositionEquals(pPosition, _s2Position)) { return OneInFront(_s0Position, new int[2] { -orientation[0], -orientation[1] } ); }

                //code should not be able to read this place, check if you are calling the function correctly
                Logging.LogInfo("this function should not be called as the player's target is not at the slope's location", Logging.debugState.SIMPLE);
                return null;
            }
            catch
            {
                Logging.LogInfo("pPosition was not given a proper value in MoveOnSlope", Logging.debugState.DETAILED);
                return null;
            }
        }

        /// <summary>
        /// (Leo)  A function to determine one in front of the object it is currently facing.
        /// </summary>
        /// <param name="pPosition"></param>
        /// <param name="pOrientation"></param>
        /// <returns></returns>
        public int[] OneInFront(int[] pPosition, int[] pOrientation)
        {
            try
            {
                int[] positionInFront = new int[3];
                positionInFront[0] = pPosition[0] + pOrientation[0];  // xPosition + xOrientation
                positionInFront[1] = pPosition[1];                    // yPosition
                positionInFront[2] = pPosition[2] + pOrientation[1];  // zPosition + zOrientation

                return positionInFront;
            }
            catch
            {
                Logging.LogInfo("pPosition or pOrientation was not given a proper value in OneInFront", Logging.debugState.DETAILED);
                return null;
            }
        }

        public bool PositionEquals(int[] pPosition0, int[] pPosition1)
        {
            if (pPosition0[0] == pPosition1[0] && pPosition0[1] == pPosition1[1] && pPosition0[2] == pPosition1[2]) { return true; }
            return false;
        }
    }
}
