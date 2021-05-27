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


            //GenerateGridFromText("../../../../LevelFiles/pogLevel.txt");
            GenerateGridFromText("../../../../LevelFiles/3DPrototype.txt");


            //SpawnPoint _spawnPoint0 = new SpawnPoint(this, 1, 0, 1, 0);
            //spawnPoints.Add(_spawnPoint0);


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

                    if (null!= _spawnPoint) SetPlayerCoord(pListener, _spawnPoint.position[0], _spawnPoint.position[1], _spawnPoint.position[2], 0);
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

                    if (null != _spawnPoint) SetPlayerCoord(pListener, _spawnPoint.position[0], _spawnPoint.position[1], _spawnPoint.position[2], 1);
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
