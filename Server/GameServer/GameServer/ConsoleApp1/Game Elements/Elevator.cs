using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Elevator : GameObject
    {
        public int ID;
        //public List<GameObject> point = new List<GameObject>();
        private int currentPos;
        private int oldPos;
        public Dictionary<int, EmptyGameObject> points = new Dictionary<int, EmptyGameObject>();


        GameRoom _room;

        public Elevator(GameRoom pRoom, int pX, int pY, int pID) : base(pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(9);
            objectIndex = 9;
            currentPos = 0;
        }


        public void NextPosition(Button.Direction currentDirection)
        {
            ConfElevatorMove elevatorMove = new ConfElevatorMove();

            if (currentDirection == Button.Direction.DOWN)
            {
                oldPos = currentPos;

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

                _room.coordinatesAdd(points[currentPos].position[0], points[currentPos].position[1], 9);
                _room.coordinatesRemove(points[oldPos].position[0], points[oldPos].position[1], 9);
                _room.sendToAll(elevatorMove);
            }

            else if (currentDirection == Button.Direction.UP)
            {

                oldPos = currentPos;

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

                _room.coordinatesAdd(points[currentPos].position[0], points[currentPos].position[1], 9);
                _room.coordinatesRemove(points[oldPos].position[0], points[oldPos].position[1], 9);
                _room.sendToAll(elevatorMove);
            }
            else
            {
                Logging.LogInfo("Button.cs: Cannot handle next position, probably because currentDirection is null", Logging.debugState.DETAILED);
            }
        }

    }
}
