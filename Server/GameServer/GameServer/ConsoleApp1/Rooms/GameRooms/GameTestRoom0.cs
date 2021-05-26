using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class GameTestRoom0 : GameRoom
    {
        public GameTestRoom0(TCPGameServer pServer, int pWidth, int pHeight, int pLength) : base(pServer, pWidth, pHeight, pLength)
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

            //generateGridFromText("../../../../LevelFiles/actualCoolLevel.txt");


            GenerateGridFromText("../../../../LevelFiles/pogLevel.txt");


            SpawnPoint _spawnPoint0 = new SpawnPoint(this, 0, 0, 0, 0);
            spawnPoints.Add(_spawnPoint0);

            CopyGrid(roomStatic, roomArray);
            PrintGrid(roomArray);

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
