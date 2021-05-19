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
            Wall wall0 = new Wall(this, 0, 5);
            Wall wall1 = new Wall(this, 1, 5);
            Wall wall2 = new Wall(this, 2, 5);
            Wall wall3 = new Wall(this, 3, 5);
            Wall wall4 = new Wall(this, 4, 5);
            
            SpawnPoint _spawnPoint0 = new SpawnPoint(this, 0, 0, 0);
            spawnPoints.Add(_spawnPoint0);

            SpawnPoint _spawnPoint1 = new SpawnPoint(this, 1, 1, 0);
            spawnPoints.Add(_spawnPoint1);
            CopyGrid(roomStatic, roomArray);
        }

        public override void AddMember(TCPMessageChannel pListener)
        {
            base.AddMember(pListener);
            if (_users.Count < 1)
            {
                SetPlayerCoord(pListener, 0, 0);
            }

            else
            {
                SetPlayerCoord(pListener, 1, 0);
            }
        }
    }
}
