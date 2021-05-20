using sharedAngy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public abstract class GameRoom : Room
    {
        public List<int>[,] roomArray;
        public List<int>[,] roomStatic;
        //quick cheat sheet
        //0 is empty
        //1 is player
        //2 is wall
        //3 is spawn point
        //4 is lever
        //5 is pressure plate
        //6 is door

        public List<GameObject> gameObjects;
        public List<Player> players;
        public List<SpawnPoint> spawnPoints;
 
        public GameRoom(TCPGameServer pServer, int roomWidth, int roomHeight) : base(pServer)
        {
            roomArray = new List<int>[roomWidth, roomHeight];
            


            roomStatic = new List<int>[roomWidth, roomHeight];
            initializeAllLists();
            players = new List<Player>();
            spawnPoints = new List<SpawnPoint>();
            gameObjects = new List<GameObject>();
        }
        private void initializeAllLists()
        {
            for (int y = 0; y < roomArray.GetLength(0); y++)
            {
                for (int x = 0; x < roomArray.GetLength(1); x++)
                {
                    roomArray[x, y] = new List<int>();
                }
                
            }

            for (int y = 0; y < roomStatic.GetLength(0); y++)
            {
                for (int x = 0; x < roomStatic.GetLength(1); x++)
                {
                    roomStatic[x, y] = new List<int>();
                }
                
            }
        }

        public bool coordinatesContain(int pX, int pY, int pValue)
        {
            foreach (int value in roomArray[pX, pY])
            {
                if (value == pValue) return true;
            }

            return false;
        }

        public void coordinatesRemove(int pX, int pY, int pValue)
        {
            foreach (int value in roomArray[pX, pY])
            {
                if (value == pValue) roomArray[pX,pY].Remove(value);
            }

            
        }


        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a package! Trying to handle", Logging.debugState.SPAM);
            if (pMessage is ReqKeyDown){handleReqKeyDown(pMessage as ReqKeyDown, pSender);}
            if (pMessage is ReqKeyUp){handleReqKeyUp(pMessage as ReqKeyUp, pSender);}
            if(pMessage is ConfDoorToggle) { handleDoorToggle(pMessage as ConfDoorToggle); }
        }

        private void handleDoorToggle(ConfDoorToggle pMessage)
        {
            //roomArray[pMessage.posX, pMessage.posY] = 0;
            coordinatesRemove(pMessage.posX, pMessage.posY, 6);
        }

        private void handleReqKeyDown(ReqKeyDown pKeyDown, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a HandleReqKeyDown Package", Logging.debugState.DETAILED);
            foreach (Player player in players)
            {
                if (pSender == player.getClient())
                {
                    player.addInput(pKeyDown.keyInput);
                }
            }
        }

        private void handleReqKeyUp(ReqKeyUp pKeyUp, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a HandleReqKeyUp Package", Logging.debugState.DETAILED);
            foreach (Player player in players)
            {
                if (pSender == player.getClient())
                {
                    player.removeInput(pKeyUp.keyInput);
                }
            }
        }

        public override void AddMember(TCPMessageChannel pListener)
        {
            Logging.LogInfo("Added client to gameRoom0");
            base.AddMember(pListener);
        }

        protected void SetPlayerCoord(TCPMessageChannel pListener, int pX, int pY)
        {
            players.Add(new Player(this, pListener, pX, pY));
        }

        public void printGrid(List<int>[,] pGrid)
        {
            for (int y = 0; y < pGrid.GetLength(0); y++)
            {
                for (int x = 0; x < pGrid.GetLength(1); x++)
                {
                    Console.Write(pGrid[x, y]);
                }
                Console.Write("\n");
            }
        }

        public void ResetRoom()
        {
            Logging.LogInfo("Current Grid");
            printGrid(roomArray);

            Logging.LogInfo("\n\nSaved Grid");
            printGrid(roomStatic);
            CopyGrid(roomArray, roomStatic);

            RespawnPlayer();
        }

        public void CopyGrid(List<int>[,] pGrid0, List<int>[,] pGrid1)
        {
            try
            {
                for (int y = 0; y < pGrid0.GetLength(0); y++)
                {
                    for (int x = 0; x < pGrid0.GetLength(1); x++)
                    {
                        pGrid0[x, y] = pGrid1[x, y];
                    }
                }
            }
            catch (Exception e)
            {
                Logging.LogInfo(e.Message);
                Logging.LogInfo("The grids were probably not equal in size", Logging.debugState.DETAILED);
            }
        }
        public void RespawnPlayer()
        {
            try
            {
                
                for (int i = 0; i < players.Count; i++)
                {
                    Logging.LogInfo("teleporting to:  ( " + spawnPoints[i].position[0] + ", " + spawnPoints[i].position[1] + ")");
                    players[i].position[0] = spawnPoints[i].position[0];
                    players[i].position[1] = spawnPoints[i].position[1];
                }
            }
            catch (Exception e)
            {
                Logging.LogInfo("The amount of players/checkpoints is probably off when respawning");
                Logging.LogInfo(e.Message, Logging.debugState.DETAILED);
            }
        }

        
        public override void RemoveMember(TCPMessageChannel pListener)
        {
            //get index 
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i] == pListener)
                {
                    players.Remove(players[i]);
                }
            }
            base.RemoveMember(pListener);
            
        }

        public override void Update()
        {
            base.Update();
            foreach(GameObject obj in gameObjects)
            {
                obj.Update();
            }
        }

    }
}
