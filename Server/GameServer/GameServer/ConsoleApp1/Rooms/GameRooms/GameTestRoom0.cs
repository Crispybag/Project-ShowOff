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
            //GenerateGridFromText("../../../../LevelFiles/TestSpawnPoint.txt");
            GenerateGridFromText("../../../../LevelFiles/3DPrototype.txt");
            CopyGrid(roomStatic, roomArray);
            PrintGrid(roomArray);
        }

        public override void AddMember(TCPMessageChannel pListener)
        {
            base.AddMember(pListener);
            
            bool player1Connected = false;
            foreach (Player player in players)
            {
                if (player.GetPlayerIndex() == 0) player1Connected = true;
            }

            if (!player1Connected)
            {
                try
                {
                    //spawn player at spawn point if there is one
                    List<GameObject> _spawnPoints = OnCoordinatesFindGameObjectOfType(3);
                    SpawnPoint _spawnPoint = null;
                    
                    foreach (GameObject obj in _spawnPoints)
                    {
                        SpawnPoint smiley = obj as SpawnPoint;
                        if (smiley.spawnIndex == 0) _spawnPoint = smiley;
                    }

                    if (null!= _spawnPoint) SetPlayerCoord(pListener, _spawnPoint.x(), _spawnPoint.y(), _spawnPoint.z(), 0);
                    PrintGrid(roomArray);
                }
                catch
                {
                    Logging.LogInfo("AAAAAAAAAAAAAAAAAAAAAAA something went wrong lol (in gametestroom0 adding member)", Logging.debugState.SIMPLE);
                }
            }

            else
            {
                try
                {
                    //spawn player at spawn point if there is one
                    List<GameObject> _spawnPoints = OnCoordinatesFindGameObjectOfType(3);
                    SpawnPoint _spawnPoint = new SpawnPoint(this, 0, 0, 0, 0);

                    foreach (GameObject obj in _spawnPoints)
                    {
                        SpawnPoint smiley = obj as SpawnPoint;
                        if (smiley.spawnIndex == 1) _spawnPoint = smiley;
                    }

                    if (null != _spawnPoint) SetPlayerCoord(pListener, _spawnPoint.x(), _spawnPoint.y(), _spawnPoint.z(), 1);
                    PrintGrid(roomArray);
                }
                catch
                {
                    Logging.LogInfo("AAAAAAAAAAAAAAAAAAAAAAA something went wrong lol (in gametestroom0 adding member)", Logging.debugState.SIMPLE);
                }

                //SetPlayerCoord(pListener, 9, 0, 0, 1);
            }
        }
    }
}
