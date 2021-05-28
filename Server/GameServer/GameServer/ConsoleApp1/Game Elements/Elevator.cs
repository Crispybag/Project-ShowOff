using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) This class implements an elevator, with positions it can go to
    /// </summary>
    class Elevator : GameObject
    {
        public int ID;
        private int currentPos;
        private int oldPos;
        //contains the points the elevator can go in
        public Dictionary<int, EmptyGameObject> points = new Dictionary<int, EmptyGameObject>();


        public Elevator(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            room = pRoom;
            room.roomArray[position[0], position[1], position[2]].Add(9);
            objectIndex = 9;
            currentPos = 0;
        }

        /// <summary>
        /// (Ezra) Changes position based on input
        /// </summary>
        public void NextPosition(Button.Direction currentDirection)
        {
            ConfElevatorMove elevatorMove = new ConfElevatorMove();

            if (currentDirection == Button.Direction.DOWN)
            {
                oldPos = currentPos;

                //makes sure it doesnt go outside the dictonairy
                if (currentPos <= 0)
                {
                    currentPos = 0;
                }
                else
                {
                    currentPos--;
                }

                elevatorMove.ID = ID;
                elevatorMove.posX = points[currentPos].position[0];
                elevatorMove.posY = points[currentPos].position[1];
                elevatorMove.posZ = points[currentPos].position[2];

                room.OnCoordinatesAdd(points[currentPos].position[0], points[currentPos].position[1], points[currentPos].position[2], 9);
                room.OnCoordinatesRemove(points[oldPos].position[0], points[oldPos].position[1], points[currentPos].position[2], 9);
                room.sendToAll(elevatorMove);
            }

            else if (currentDirection == Button.Direction.UP)
            {

                oldPos = currentPos;

                //makes sure it doesnt go outside the dictonairy
                if (currentPos >= points.Count -1)
                {
                    currentPos = points.Count -1;
                }
                else
                {
                    currentPos++;
                }

                elevatorMove.ID = ID;
                elevatorMove.posX = points[currentPos].position[0];
                elevatorMove.posY = points[currentPos].position[1];
                elevatorMove.posZ = points[currentPos].position[2];

                room.OnCoordinatesAdd(points[currentPos].position[0], points[currentPos].position[1], points[currentPos].position[2], 9);
                room.OnCoordinatesRemove(points[oldPos].position[0], points[oldPos].position[1], points[currentPos].position[2], 9);
                room.sendToAll(elevatorMove);
            }
            else
            {
                Logging.LogInfo("Button.cs: Cannot handle next position, probably because currentDirection is null", Logging.debugState.DETAILED);
            }
        }

    }
}
