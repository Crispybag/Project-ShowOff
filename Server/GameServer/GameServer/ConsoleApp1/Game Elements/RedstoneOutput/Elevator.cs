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
        public bool isGoingUp = true;

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
        public void NextPosition()
        {
            ConfElevatorMove elevatorMove = new ConfElevatorMove();
            
            oldPos = currentPos;

            //makes sure it doesnt go outside the dictonairy
            if (currentPos >= points.Count - 1)
            {
                isGoingUp = false;
            }
            else if(currentPos <= 0)
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
                        Box coolBox = room.OnCoordinatesGetGameObject(oldX, oldY + 1, oldZ, 7) as Box;
                        coolBox.MovePosition(x(), y() + 1, z());
                        coolBox.sendBoxPackage(false);
                    }
                }
            }
            if (room.OnCoordinatesContain(oldX, oldY + 1, oldZ, 7))
            {
                if (room.OnCoordinatesGetGameObject(oldX, oldY + 1, oldZ, 7) is Box)
                {
                    Box coolBox = room.OnCoordinatesGetGameObject(oldX, oldY + 1, oldZ, 7) as Box;
                    coolBox.MovePosition(x(), y() + 1, z());
                    coolBox.sendBoxPackage(false);
                }
            }
            elevatorMove.ID = ID;
            elevatorMove.posX = points[currentPos].x() + room.minX;
            elevatorMove.posY = points[currentPos].y() + room.minY;
            elevatorMove.posZ = points[currentPos].z() + room.minZ;  

            room.sendToAll(elevatorMove);
        }

    }
}
