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


        public Elevator(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
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

                int oldX = x();
                int oldY = y();
                int oldZ = z();
                MovePosition(points[currentPos].x(), points[currentPos].y(), points[currentPos].z());

                Player playerOnElevator;
                if (room.OnCoordinatesContain(oldX, oldY + 1, oldZ, 1))
                {
                    if (room.OnCoordinatesGetGameObject(oldX, oldY + 1, oldZ, 1) is Player)
                    {
                        playerOnElevator = room.OnCoordinatesGetGameObject(oldX, oldY + 1, oldZ, 1) as Player;
                        try
                        {
                            playerOnElevator.tryPositionChange(points[currentPos].x() - playerOnElevator.x(), points[currentPos].y() + 1 - playerOnElevator.y(), points[currentPos].z() - playerOnElevator.z());
                        }
                        catch
                        {
                            Logging.LogInfo("I am really sad, the index falls out of bounds :(", Logging.debugState.DETAILED);
                        }
                    }
                }

                elevatorMove.ID = ID;
                elevatorMove.posX = points[currentPos].x() + room.minX;
                elevatorMove.posY = points[currentPos].y() + room.minY;
                elevatorMove.posZ = points[currentPos].z() + room.minZ;

                //room.OnCoordinatesAdd(points[currentPos].position[0], points[currentPos].position[1], points[currentPos].position[2], 9);
                //room.OnCoordinatesRemove(points[oldPos].position[0], points[oldPos].position[1], points[currentPos].position[2], 9);                

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
                int oldX = x();
                int oldY = y();
                int oldZ = z();

                MovePosition(points[currentPos].x(), points[currentPos].y(), points[currentPos].z());


                Player playerOnElevator;
                if (room.OnCoordinatesContain(oldX, oldY + 1, oldZ, 1))
                {
                    if (room.OnCoordinatesGetGameObject(oldX, oldY + 1, oldZ, 1) is Player)
                    {
                        playerOnElevator = room.OnCoordinatesGetGameObject(oldX, oldY + 1, oldZ, 1) as Player;
                        try
                        {
                            playerOnElevator.tryPositionChange(points[currentPos].x() - playerOnElevator.x(), points[currentPos].y() + 1 - playerOnElevator.y(), points[currentPos].z() - playerOnElevator.z());
                        }
                        catch
                        {
                            Logging.LogInfo("I am really sad, the index falls out of bounds :(", Logging.debugState.DETAILED);
                        }
                    }
                }

                elevatorMove.ID = ID;
                elevatorMove.posX = points[currentPos].x();
                elevatorMove.posY = points[currentPos].y();
                elevatorMove.posZ = points[currentPos].z();

                room.sendToAll(elevatorMove);
            }
            else
            {
                Logging.LogInfo("Button.cs: Cannot handle next position, probably because currentDirection is null", Logging.debugState.DETAILED);
            }
        }

    }
}
