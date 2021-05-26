using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class GameTestRoom0 : GameRoom
    {
        public GameTestRoom0(TCPGameServer pServer, int pWidth, int pHeight) : base(pServer, pWidth, pHeight)
        {

            //quick cheat sheet
            //0 is empty
            //1 is player
            //2 is wall
            //3 is spawn point
            //4 is actuator
            //5 is pressure plate
            //6 is door
            //7 is box
            //8 is button


            Wall wall0 = new Wall(this, 0, 3);
            Wall wall1 = new Wall(this, 1, 3);
            //Wall doorish = new Wall(this, 2, 3);
            Wall wall2 = new Wall(this, 3, 3);

            Wall wall3 = new Wall(this, 4, 0);
            Wall wall4 = new Wall(this, 4, 1);
            Wall wall5 = new Wall(this, 4, 2);
            Wall wall6 = new Wall(this, 4, 3);
            Wall wall7 = new Wall(this, 4, 4);
            Wall wall8 = new Wall(this, 4, 5);

            Wall wall9 = new Wall(this, 5, 5);
            Wall wall10 = new Wall(this, 6, 5);
            Wall wall11 = new Wall(this, 7, 5);
            Wall wall12 = new Wall(this, 8, 5);

            Wall wall13 = new Wall(this, 0, 7);
            Wall wall14 = new Wall(this, 1, 7);
            Wall wall15 = new Wall(this, 2, 7);
            Wall wall16 = new Wall(this, 2, 9);

            Lever lever0 = new Lever(this, 5, 2, 1, false);
            Lever lever1 = new Lever(this, 9, 9, 2, false);
            Lever lever2 = new Lever(this, 0, 9, 3, false);

            Door door0 = new Door(this, 2, 3, 4, false);
            Door door1 = new Door(this, 9, 5, 5, false);
            Door door2 = new Door(this, 2, 8, 6, false);
            Door doorVictory = new Door(this, 2, 8, 7, false);

            PressurePlate pressurePlate0 = new PressurePlate(this, 7, 4, 8, false);
            PressurePlate pressurePlate1 = new PressurePlate(this, 0, 6, 9, false);

            Button button0 = new Button(this, 3, 9, 10);
            Button button1 = new Button(this, 7, 6, 11);

            Elevator elevator0 = new Elevator(this, 5, 6, 12);
            EmptyGameObject empty0 = new EmptyGameObject(this,5,6);
            EmptyGameObject empty1 = new EmptyGameObject(this,7,7);
            EmptyGameObject empty2 = new EmptyGameObject(this,5,9);
            elevator0.points.Add(0,empty0);
            elevator0.points.Add(1,empty1);
            elevator0.points.Add(2,empty2);

            button0.currentDirection = Button.Direction.DOWN;
            button1.currentDirection = Button.Direction.UP;

            button0.elevators.Add(elevator0);
            button1.elevators.Add(elevator0);

            pressurePlate0.doors.Add(door2);
            door2.actuators.Add(pressurePlate0);

            pressurePlate1.doors.Add(door1);
            door1.actuators.Add(pressurePlate1);

            lever0.doors.Add(door0);
            lever1.doors.Add(door1);
            lever1.doors.Add(door2);
            lever2.doors.Add(doorVictory);

            door0.actuators.Add(lever0);
            door1.actuators.Add(lever1);
            door2.actuators.Add(lever1);
            doorVictory.actuators.Add(lever2);

            Box box = new Box(this, 1, 2);


            generateGridFromText("../../../../LevelFiles/actualCoolLevel.txt");

            SpawnPoint _spawnPoint0 = new SpawnPoint(this, 0, 0, 0);
            spawnPoints.Add(_spawnPoint0);

            //SpawnPoint _spawnPoint1 = new SpawnPoint(this, 1, 9, 0);
            //spawnPoints.Add(_spawnPoint1);

            CopyGrid(roomStatic, roomArray);
            printGrid(roomArray);

        }

        public override void AddMember(TCPMessageChannel pListener)
        {
            base.AddMember(pListener);
            if (_users.Count <= 1)
            {
                SetPlayerCoord(pListener, 9, 0);
            }

            else
            {
                SetPlayerCoord(pListener, 0, 0);
            }
        }
    }
}
