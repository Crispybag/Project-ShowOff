using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    public class Slope : GameObject
    {
        private GameRoom _room;

        //rotation of the slope
        private int[] orientation;
        
        //since slopes are 2 * 1 * 1 it takes up 2 positions
        private int[] _s0Position;
        private int[] _s1Position;
        private int[] _s2Position;

        #region intialization
        public Slope(GameRoom room, int pX, int pY, int pZ, int rotation) : base(room, CollInteractType.PASS)
        {
            _room = room;

            //set the base position of the gmae object
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;

            //set the orientation based on the y-rotation of the objects
            orientation = new int[2];
            switch (rotation)
            {
                case (0):
                    orientation[0] = 0; orientation[1] = -1;
                    break;

                case (90):
                    orientation[0] = 1; orientation[1] = 0;
                    break;

                case (180):
                    orientation[0] = 0; orientation[1] = 1;
                    break;

                case (270):
                    orientation[0] = -1; orientation[1] = 0;
                    break;

                case (-90):
                    orientation[0] = -1; orientation[1] = 0;
                    break;

                default:
                    Logging.LogInfo("found a slope at a non 90 degree angle, bruh", Logging.debugState.SIMPLE);
                    break;

            }

            //set positions to respective parts
            _s0Position = new int[3] { position[0], position[1], position[2] };                                  //
            _s1Position = new int[3] { position[0] + orientation[0], position[1], position[2] + orientation[1]}; //
            _s2Position = new int[3] { _s1Position[0], _s1Position[1] + 1, _s1Position[2] };                        //position above s1
            
            //load objects in the scene
            _room.roomArray[_s0Position[0], _s0Position[1], _s0Position[2]].Add(10);
            _room.roomArray[_s1Position[0], _s1Position[1], _s1Position[2]].Add(11);
            _room.roomArray[_s2Position[0], _s2Position[1], _s2Position[2]].Add(12);
        }
        #endregion

        /// <summary>
        /// Checks if the player can move on the slope or not based on the position it is trying to hit the slope and the orientation the player currently has
        /// </summary>
        /// <param name="pPosition"></param>
        /// <param name="pOrientation"></param>
        /// <returns></returns>
        public bool CanMoveOnSlope(int[] pPosition, int[] pOrientation)
        {
            //if it hits position 2 and has opposing orientation
            if      (pPosition == _s2Position && pOrientation[0] == -orientation[0] && pOrientation[1] == -pOrientation[1]) 
            {
                try 
                { 
                //make sure that the room the player wants to move to is empty and exists
                if (_room.coordinatesEmpty(OneInFront(_s0Position, orientation))) { return true; }
                    return false;
                
                }
                catch
                {
                    Logging.LogInfo("Slope tries to send player outside of bounds of array in CanMoveOnSlope", Logging.debugState.DETAILED);
                    return false;
                }
            }

            //if it hits position 0 and has same orientation
            else if (pPosition == _s0Position && pOrientation[0] == orientation[0] && pOrientation[1] == orientation[1]) 
            {
                try
                {
                    //make sure that the room the player wants to move to is empty and exists
                    if (_room.coordinatesEmpty(OneInFront(_s2Position, orientation))) { return true; }
                    return false;

                }
                catch
                {
                    Logging.LogInfo("Slope tries to send player outside of bounds of array in CanMoveOnSlope", Logging.debugState.DETAILED);
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// move the player on the slope based on which of the 2 positions the player has hit
        /// </summary>
        /// <param name="pPosition"></param>
        /// <returns></returns>
        public int[] MoveOnSlope(int[] pPosition)
        {
            try
            {
                //if position is s0, move it one past s2
                if (pPosition == _s0Position) { return OneInFront(_s2Position, orientation); }

                //if position is s2, move it one past s0
                else if (pPosition == _s2Position) { return OneInFront(_s0Position, new int[2] { -orientation[0], -orientation[1] } ); }

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
        /// a function to determine one in front of the object it is currently facing.
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
                positionInFront[1] = pPosition[1];                   // yPosition
                positionInFront[2] = pPosition[2] + pOrientation[1];  // zPosition + zOrientation

                return positionInFront;
            }
            catch
            {
                Logging.LogInfo("pPosition or pOrientation was not given a proper value in OneInFront", Logging.debugState.DETAILED);
                return null;
            }
        }
    }
}
