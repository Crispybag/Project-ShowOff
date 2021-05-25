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
            Wall wall0 = new Wall(this, 0, 3);
            Wall wall1 = new Wall(this, 1, 3);
            Wall doorish = new Wall(this, 2, 3);
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
            Wall doorish1 = new Wall(this, 9, 5);

            Wall wall13 = new Wall(this, 0, 7);
            Wall wall14 = new Wall(this, 1, 7);
            Wall wall15 = new Wall(this, 2, 7);
            Wall doorish2 = new Wall(this, 2, 8);
            Wall wall16 = new Wall(this, 2, 9);

            Lever lever0 = new Lever(this, 5, 2, 1, false);
            Lever lever1 = new Lever(this, 9, 9, 2, false);
            Lever lever2 = new Lever(this, 0, 9, 3, false);

            Door door0 = new Door(this, 3, 2);
            lever0.doors.Add(door0);

            Box box = new Box(this, 1, 2);

            SpawnPoint _spawnPoint0 = new SpawnPoint(this, 0, 0, 0);
            spawnPoints.Add(_spawnPoint0);

            SpawnPoint _spawnPoint1 = new SpawnPoint(this, 1, 9, 0);
            spawnPoints.Add(_spawnPoint1);

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
