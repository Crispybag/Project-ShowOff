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
        //7 is box

        public List<GameObject> gameObjects;
        public List<Player> players;
        public List<SpawnPoint> spawnPoints;

        #region initialization
        public GameRoom(TCPGameServer pServer, int roomWidth, int roomHeight) : base(pServer)
        {
            //set up rooms
            roomArray = new List<int>[roomWidth, roomHeight];
            roomStatic = new List<int>[roomWidth, roomHeight];

            //initialise every list in list array
            initializeAllLists();

            //set up miscellaneous lists
            players = new List<Player>();
            spawnPoints = new List<SpawnPoint>();
            gameObjects = new List<GameObject>();

        }

        //makes a new list for every single list in the arrays
        private void initializeAllLists()
        {
            //Make all lists for room array
            for (int y = 0; y < roomArray.GetLength(0); y++)
            {
                for (int x = 0; x < roomArray.GetLength(1); x++)
                {
                    roomArray[x, y] = new List<int>();
                }
                
            }

            //Make all lists for room static
            for (int y = 0; y < roomStatic.GetLength(0); y++)
            {
                for (int x = 0; x < roomStatic.GetLength(1); x++)
                {
                    roomStatic[x, y] = new List<int>();
                }
                
            }
        }

        #endregion

        #region grid tools
        /// <summary>
        /// checks if the coordinates have a certain value
        /// </summary>
        /// <param name="pX"> x-coordinate</param>
        /// <param name="pY"> y-coordinate</param>
        /// <param name="pValue"> value on coordinate </param>
        /// <returns>true if the coordinates contain the value</returns>
        public bool coordinatesContain(int pX, int pY, int pValue)
        {
            foreach (int value in roomArray[pX, pY])
            {
                if (value == pValue) return true;
            }

            return false;
        }
        /// <summary>
        /// Checks if coordinates[pX, pY] is empty
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <returns>true if list count is 0</returns>
        public bool coordinatesEmpty(int pX, int pY)
        {
            if (roomArray[pX, pY].Count == 0) return true;
            return false;
        }

        /// <summary>
        /// quick tool function to remove all values from given value
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void coordinatesRemove(int pX, int pY, int pValue)
        {
            List<int> thingToRemove = new List<int>();
            foreach (int value in roomArray[pX, pY])
            {
                if (value == pValue)
                {
                    thingToRemove.Add(value);
                    
                }
            }

            foreach(int removedValue in thingToRemove)
            {
                roomArray[pX, pY].Remove(removedValue);
            }
        }

        /// <summary>
        /// Get game object of index on specific coordinate
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="index">index type of the gameobject</param>
        /// <returns></returns>
        public GameObject coordinatesGetGameObject(int pX, int pY, int index)
        {
            //check if the coordinate does indeed containt the index
            if (coordinatesContain(pX,pY,index))
            {
                //go through game object list and see if you can get the right index
                foreach (GameObject obj in gameObjects)
                {
                    //make sure the type and coordinates are the same
                    if (obj.position[0] == pX && obj.position[1] == pY && obj.objectIndex == index)
                    {
                        //yay you get the game object
                        return obj;
                    }
                    
                }
                Logging.LogInfo("When trying to look for the game object it could not be found", Logging.debugState.DETAILED);
                return null;
            }

            else
                Logging.LogInfo("coordinate does not contain the wished for index", Logging.debugState.DETAILED);

            return null;
        }


        //Tool that prints the entire grid in the console, for debugging purposes (not yet tested with list printing)
        public void printGrid(List<int>[,] pGrid)
        {
            for (int y = 0; y < pGrid.GetLength(0); y++)
            {
                for (int x = 0; x < pGrid.GetLength(1); x++)
                {
                    int reverseY = pGrid.GetLength(0) - y - 1;
                    Console.Write("[ ");
                    for (int element = 0; element < pGrid[x, reverseY].Count; element++)
                    {
                        if (element != 0) Console.Write(", ");
                        else Console.Write("  ");
                        Console.Write(pGrid[x, reverseY][element]);
                    }

                    if (pGrid[x, reverseY].Count < 3)
                    {
                        for (int element = 0; element < 3 - pGrid[x, reverseY].Count; element++)
                        {
                            Console.Write("   ");

                        }
                    }
                    Console.Write(" ]");
                }
                Console.Write("\n");
            }
        }

        //Copy the values of grid 1 to grid 0. Mostly used for resetting the level
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
        #endregion

        #region network handling

        //Receives a package and determine how to open the package
        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a package! Trying to handle", Logging.debugState.SPAM);
            if (pMessage is ReqKeyDown){handleReqKeyDown(pMessage as ReqKeyDown, pSender);}
            if (pMessage is ReqKeyUp){handleReqKeyUp(pMessage as ReqKeyUp, pSender);}
            if(pMessage is ConfDoorToggle) { handleDoorToggle(pMessage as ConfDoorToggle); }
        }

        //handle door toggle
        private void handleDoorToggle(ConfDoorToggle pMessage)
        {
            coordinatesRemove(pMessage.posX, pMessage.posY, 6);
        }

        //handle request key down package
        private void handleReqKeyDown(ReqKeyDown pKeyDown, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a HandleReqKeyDown Package", Logging.debugState.DETAILED);
            
            //search which player should be influenced by this package send
            foreach (Player player in players)
            {
                if (pSender == player.getClient())
                {
                    //goes to player for further processing of the package
                    player.addInput(pKeyDown.keyInput);
                }
            }
        }

        //handle request key up package
        private void handleReqKeyUp(ReqKeyUp pKeyUp, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a HandleReqKeyUp Package", Logging.debugState.DETAILED);
            
            //search which player should be influenced by the package send
            foreach (Player player in players)
            {
                if (pSender == player.getClient())
                {
                    //goes to player for further processing of the package
                    player.removeInput(pKeyUp.keyInput);
                }
            }
        }
        #endregion

        #region resetting
        //Teleports player to certain coordinates
        protected void SetPlayerCoord(TCPMessageChannel pListener, int pX, int pY)
        {
            players.Add(new Player(this, pListener, pX, pY));
        }


        //Reset the room to the initial state
        public void ResetRoom()
        {
            //for debug purposes print grid
            Logging.LogInfo("Current Grid");
            printGrid(roomArray);

            Logging.LogInfo("\n\nSaved Grid");
            printGrid(roomStatic);

            //copy static grid to room grid
            CopyGrid(roomArray, roomStatic);

            //reset player positions
            RespawnPlayer();

            //probably need to handle lever states or whatever aswell
        }

        //spawns a player on a checkpoint if the checkpoint is available
        public void RespawnPlayer()
        {
            try
            {
                //for each player teleport to corresponding respawn point
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


        #endregion

        #region overrides (add member/ remove member/ update)
        public override void AddMember(TCPMessageChannel pListener)
        {
            //quick extra debug info to show that the client joined a game room
            Logging.LogInfo("Added client to gameRoom");
            base.AddMember(pListener);
        }

        //in addition to removing the user from users, remove them from the player list, as well.
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

        //update each gameobject update() in this update loop
        public override void Update()
        {
            base.Update();
            foreach(GameObject obj in gameObjects)
            {
                obj.Update();
            }
        }
        #endregion

    }
}
