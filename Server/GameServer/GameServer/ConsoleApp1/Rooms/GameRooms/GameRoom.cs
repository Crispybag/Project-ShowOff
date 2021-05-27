using sharedAngy;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Server
{
    public abstract class GameRoom : Room
    {
        public List<int>[,,] roomArray;
        public List<int>[,,] roomStatic;
        //quick cheat sheet
        //0 is empty
        //1 is player
        //2 is wall
        //3 is spawn point
        //4 is actuator
        //5 is pressure plate
        //6 is door
        //7 is box

        public List<GameObject> gameObjects;
        public List<Player> players;
        public List<SpawnPoint> spawnPoints;

        #region initialization
        public GameRoom(TCPGameServer pServer, int roomWidth, int roomHeight, int roomLength) : base(pServer)
        {
            //set up rooms
            roomArray = new List<int>[roomWidth, roomHeight, roomLength];
            roomStatic = new List<int>[roomWidth, roomHeight, roomLength];

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
            for (int z = 0; z < roomArray.GetLength(2); z++)
            {
                for (int y = 0; y < roomArray.GetLength(1); y++)
                {
                    for (int x = 0; x < roomArray.GetLength(0); x++)
                    {
                        int nDebugLength = roomArray.GetLength(0);
                        int nDebugLength1 = roomArray.GetLength(1);
                        roomArray[x, y, z] = new List<int>();
                    }

                }
            }

            //Make all lists for room static
            for (int z = 0; z < roomStatic.GetLength(2); z++)
            {
                for (int y = 0; y < roomStatic.GetLength(1); y++)
                {
                    for (int x = 0; x < roomStatic.GetLength(0); x++)
                    {
                        roomStatic[x, y, z] = new List<int>();
                    }

                }
            }
        }

        #endregion

        #region level data loading

        public void GenerateGridFromText(string filePath)
        {
            //values to determine grid size
            int minX = 0, minY = 0, minZ = 0;

            //get the file path
            List<string> lines = File.ReadAllLines(filePath).ToList();
            determineGridSize(lines, out minX, out minY, out minZ);
            initializeAllLists();
            addObjectsToGrid(lines, minX, minY, minZ);

        }

        /// <summary>
        /// Determines how big the grid should be, it also returns minX and minY to reposition all items to fit into the grid
        /// </summary>
        /// <param name="information"> all information</param>
        /// <param name="minX">the lowest X value of the grid</param>
        /// <param name="minY">the lowest Y Value of the grid</param>
        private void determineGridSize(List<string> information, out int minX, out int minY, out int minZ)
        {
            int maxX = -5000000, maxY = -5000000, maxZ = -5000000;
            minX = 5000000;minY = 5000000; minZ = 500000000;
            foreach (string line in information)
            {
                //split the strings based on spaces
                string[] rawInformation = line.Split(' ');
                //determine x and y coord
                int xCoord = (int)float.Parse(rawInformation[1]);
                int yCoord = (int)float.Parse(rawInformation[2]);
                int zCoord = (int)float.Parse(rawInformation[3]);

                //determine minimum X, maximum X, minimum Y, maximumY
                if (xCoord < minX) minX = (int)xCoord;
                if (xCoord > maxX) maxX = (int)xCoord;
                if (yCoord < minY) minY = (int)yCoord;
                if (yCoord > maxY) maxY = (int)yCoord;
                if (zCoord < minZ) minZ = (int)zCoord;
                if (zCoord > maxZ) maxZ = (int)zCoord;
            }
            //set new size of room
            Logging.LogInfo("Grid Size: " + (maxX + 1 - minX) + " by " + (maxY + 1 - minY) + " by " + (maxZ + 1 - minZ),Logging.debugState.DETAILED);
            roomArray = new List<int>[maxX + 1 - minX, maxY + 1 - minY, maxZ + 1 - minZ];
        }

        private List<List<int>> createLists(string[] pRawInformation)
        {
            //working out lists 
            List<List<int>> lists = new List<List<int>>();

            //determines whether it should write values to a list
            bool isAddingToList = false;

            //temporary list that values get stored into
            List<int> tempList = new List<int>();

            foreach (string info in pRawInformation)
            {

                //signal that list has ended
                if (info == ")")
                {
                    //create a list data that is going to be added to lists
                    List<int> listData = new List<int>();

                    //stop trying to read more ints
                    isAddingToList = false;
                    foreach (int value in tempList)
                    {
                        listData.Add(value);
                    }

                    //add the list to lists and clear the temporary list
                    lists.Add(listData);
                    tempList.Clear();
                }

                //add upcoming value to a list if possible when it is adding things to the list
                if (isAddingToList)
                {
                    try
                    {
                        tempList.Add((int)float.Parse(info));
                    }
                    catch
                    {
                        Logging.LogInfo("found a non float value, which it can't parse", Logging.debugState.DETAILED);
                    }
                }

                //signal that reader needs to start paying attention for lists
                if (info == "(")
                {
                    isAddingToList = true;
                }
            }

            return lists;

        }



        /// <summary>
        /// add all objects to the grid based on the information
        /// </summary>
        /// <param name="information">all the information that we need to use</param>
        /// <param name="minX">the minimum X of the grid</param>
        /// <param name="minY">the minimum Y of the grid</param>
        private void addObjectsToGrid(List<string> information, int minX, int minY, int minZ)
        {
            foreach (string line in information)
            {
                //split information again with spaces to retrieve each coordinate
                string[] rawInformation = line.Split(' ');

                List<List<int>> informationLists = createLists(rawInformation);
            
                switch((int)float.Parse(rawInformation[0]))
                {
                    case (1):
                        //TODO: this might be an issue as we need to link a TCPChannel to a player, it is probably better to link spawn points and spawn those into the level
                        break;

                    case (2):
                        //coordinates are the information values + the minimum X to put the most left/most bottom/ most forward object in the (0,0,0) spot
                        Wall wall = new Wall(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ );
                        break;

                    case (5):
                        Lever lever = new Lever(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, 1, false);
                        break;

                    case (6):
                        //Door door = new Door(this, int.Parse(rawInformation[1]), int.Parse(rawInformation[2]), true);
                        //Door does not exist yet
                        break;

                    case (7):
                        Box box = new Box(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ);
                        break;

                    case (10):
                        Slope slope = new Slope(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, (int)float.Parse(rawInformation[7]));
                        break;
                }

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
        protected void SetPlayerCoord(TCPMessageChannel pListener, int pX, int pY, int pZ)
        {
            players.Add(new Player(this, pListener, pX, pY, pZ));
        }


        //Reset the room to the initial state
        public void ResetRoom()
        {
            //for debug purposes print grid
            Logging.LogInfo("Current Grid");
            PrintGrid(roomArray);

            Logging.LogInfo("\n\nSaved Grid");
            PrintGrid(roomStatic);

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

        #region grid tools

        /// <summary>
        /// finds a single object of given type, it only works with singletons at the moment, so if this needs an upgrade for other objects, notify me
        /// </summary>
        /// <param name="pIndex"></param>
        /// <returns></returns>
        public List<GameObject> OnCoordinatesFindGameObjectOfType(int pIndex)
        {
            List<GameObject> objList = new List<GameObject>();

            foreach (GameObject obj in gameObjects)
            {
                if (obj.objectIndex == pIndex) { objList.Add(obj); }
            }
            return objList;
        }


        /// <summary>
        /// checks if the coordinates have a certain value
        /// </summary>
        /// <param name="pX"> x-coordinate</param>
        /// <param name="pY"> y-coordinate</param>
        /// <param name="pValue"> value on coordinate </param>
        /// <returns>true if the coordinates contain the value</returns>
        public bool OnCoordinatesContain(int[] pPos, int pValue)
        {
            try
            {
                foreach (int value in roomArray[pPos[0], pPos[1], pPos[2]])
                {
                    if (value == pValue) return true;
                }

                return false;
            }
            catch
            {
                Logging.LogInfo("CoordinatesContain got a pPos value that was not 3 long, might want to recheck that", Logging.debugState.SIMPLE);
                return false;
            }

        }

        /// <summary>
        /// checks if the coordinates have a certain value
        /// </summary>
        /// <param name="pX"> x-coordinate</param>
        /// <param name="pY"> y-coordinate</param>
        /// <param name="pValue"> value on coordinate </param>
        /// <returns>true if the coordinates contain the value</returns>
        public bool OnCoordinatesContain(int pX, int pY, int pZ, int pValue)
        {
            try
            {
                foreach (int value in roomArray[pX, pY, pZ])
                {
                    if (value == pValue) return true;
                }

                return false;
            }
            catch
            {
                Logging.LogInfo("CoordinatesContain got a pPos value that was not 3 long, might want to recheck that", Logging.debugState.SIMPLE);
                return false;
            }

        }


        /// <summary>
        /// Checks if coordinates[pX, pY] is empty
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pZ">y-coordinate</param>
        /// <returns>true if list count is 0</returns>
        public bool OnCoordinatesEmpty(int[] pPos)
        {
            try
            {
                if (roomArray[pPos[0], pPos[1], pPos[2]].Count == 0) return true;
                return false;
            }
            catch
            {
                Logging.LogInfo("CoordinatesEmpty got a pPos value that was not 3 long, might want to recheck that", Logging.debugState.SIMPLE);
                return false;
            }
        }

        /// <summary>
        /// Checks if coordinates[pX, pY] is empty
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pZ">y-coordinate</param>
        /// <returns>true if list count is 0</returns>
        public bool OnCoordinatesEmpty(int pX, int pY, int pZ)
        {
            try
            {
                if (roomArray[pX, pY, pZ].Count == 0) return true;
                return false;
            }
            catch
            {
                Logging.LogInfo("CoordinatesEmpty got a pPos value that was not 3 long, might want to recheck that", Logging.debugState.SIMPLE);
                return false;
            }
        }


        /// <summary>
        /// quick tool function to remove all values from given value
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesRemove(int[] pPos, int pValue)
        {
            List<int> thingToRemove = new List<int>();

            Logging.LogInfo("GameRoom.cs: Trying to remove a object at : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + " with the value of: " + pValue, Logging.debugState.DETAILED);
            try
            {
                foreach (int value in roomArray[pPos[0], pPos[1], pPos[2]])
                {

                    if (value == pValue)
                    {
                        thingToRemove.Add(value);
                    }
                }
                foreach (int removedValue in thingToRemove)
                {
                    roomArray[pPos[0], pPos[1], pPos[2]].Remove(removedValue);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: Could not find position probably out of bounds", Logging.debugState.DETAILED);
            }
        }

        /// <summary>
        /// quick tool function to remove all values from given value
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesRemove(int pX, int pY, int pZ, int pValue)
        {
            List<int> thingToRemove = new List<int>();

            Logging.LogInfo("GameRoom.cs: Trying to remove a object at : " + pX + "," + pY + "," + pZ + " with the value of: " + pValue, Logging.debugState.DETAILED);
            try
            {
                foreach (int value in roomArray[pX, pY, pZ])
                {

                    if (value == pValue)
                    {
                        thingToRemove.Add(value);
                    }
                }
                foreach (int removedValue in thingToRemove)
                {
                    roomArray[pX, pY, pZ].Remove(removedValue);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: Could not find position probably out of bounds", Logging.debugState.DETAILED);
            }
        }


        /// <summary>
        /// Adds a new objects (int) to given position
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesAdd(int[] pPos, int pValue)
        {
            roomArray[pPos[2], pPos[1], pPos[2]].Add(pValue);
        }

        /// <summary>
        /// Adds a new objects (int) to given position
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesAdd(int pX, int pY, int pZ, int pValue)
        {
            roomArray[pX, pY, pZ].Add(pValue);
        }


        /// <summary>
        /// Gets all objects from the location
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        public List<int> OnCoordinatesGetIndexes(int[] pPos)
        {
            List<int> objects = new List<int>();
            foreach (int value in roomArray[pPos[0], pPos[1], pPos[2]])
            {
                objects.Add(value);
            }
            return objects;
        }

        /// <summary>
        /// Gets all objects from the location
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        public List<int> OnCoordinatesGetIndexes(int pX, int pY, int pZ)
        {
            List<int> objects = new List<int>();
            foreach (int value in roomArray[pX, pY, pZ])
            {
                objects.Add(value);
            }
            return objects;
        }


        /// Get game object of index on specific coordinate
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="index">index type of the gameobject</param>
        /// <returns></returns>
        public GameObject OnCoordinatesGetGameObject(int[] pPos, int index)
        {
            //check if the coordinate does indeed containt the index
            if (OnCoordinatesContain(pPos[0], pPos[1], pPos[2], index))
            {
                //go through game object list and see if you can get the right index
                foreach (GameObject obj in gameObjects)
                {
                    //make sure the type and coordinates are the same
                    if (obj.position[0] == pPos[0] && obj.position[1] == pPos[1] && obj.position[2] == pPos[2] && obj.objectIndex == index)
                    {
                        //yay you get the game object
                        return obj;
                    }

                }
                Logging.LogInfo("GameRoom.cs: When trying to look for the game object it could not be found", Logging.debugState.DETAILED);
                return null;
            }

            else
                Logging.LogInfo("GameRoom.cs: coordinate does not contain the wished for index", Logging.debugState.DETAILED);

            return null;
        }

        /// Get game object of index on specific coordinate
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="index">index type of the gameobject</param>
        /// <returns></returns>
        public GameObject OnCoordinatesGetGameObject(int pX, int pY, int pZ, int index)
        {
            //check if the coordinate does indeed containt the index
            if (OnCoordinatesContain(pX, pY, pZ, index))
            {
                //go through game object list and see if you can get the right index
                foreach (GameObject obj in gameObjects)
                {
                    //make sure the type and coordinates are the same
                    if (obj.position[0] == pX && obj.position[1] == pY && obj.position[2] == pZ && obj.objectIndex == index)
                    {
                        //yay you get the game object
                        return obj;
                    }

                }
                Logging.LogInfo("GameRoom.cs: When trying to look for the game object it could not be found", Logging.debugState.DETAILED);
                return null;
            }

            else
                Logging.LogInfo("GameRoom.cs: coordinate does not contain the wished for index", Logging.debugState.DETAILED);

            return null;
        }


        /// <summary>
        /// Get all game objects of index(if specified) on specific coordinate
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pIndex">index type of the gameobject</param>
        /// <returns></returns>
        public List<GameObject> OnCoordinatesGetGameObjects(int[] pPos, int pIndex = -1)
        {
            List<GameObject> objectList = new List<GameObject>();
            foreach (GameObject obj in gameObjects)
            {
                //make sure the type and coordinates are the same
                if (obj.position[0] == pPos[2] && obj.position[1] == pPos[1] && obj.position[2] == pPos[2] && (pIndex == -1 || obj.objectIndex == pIndex))
                {
                    //yay you get the game object
                    objectList.Add(obj);

                }
            }
            return objectList;
        }

        /// <summary>
        /// Get all game objects of index(if specified) on specific coordinate
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pIndex">index type of the gameobject</param>
        /// <returns></returns>
        public List<GameObject> OnCoordinatesGetGameObjects(int pX, int pY, int pZ, int pIndex = -1)
        {
            List<GameObject> objectList = new List<GameObject>();
            foreach (GameObject obj in gameObjects)
            {
                //make sure the type and coordinates are the same
                if (obj.position[0] == pX && obj.position[1] == pY && obj.position[2] == pZ && (pIndex == -1 || obj.objectIndex == pIndex))
                {
                    //yay you get the game object
                    objectList.Add(obj);

                }
            }
            return objectList;
        }


        /// <summary>
        /// Tool that prints the entire grid in the console, for debugging purposes (not yet tested with list printing)
        /// </summary>
        /// <param name="pGrid"></param>
        ///<returns></returns>
        public void PrintGrid(List<int>[,,] pGrid)
        {
            for (int y = 0; y < pGrid.GetLength(1); y++)
            {
                int reverseY = pGrid.GetLength(1) - y - 1;
                for (int z = 0; z < pGrid.GetLength(2); z++)
                {
                    for (int x = 0; x < pGrid.GetLength(0); x++)
                    {
                        int reverseZ = pGrid.GetLength(2) - z - 1;
                        Console.Write("[ ");
                        for (int element = 0; element < pGrid[x, reverseY, reverseZ].Count; element++)
                        {
                            if (element != 0) Console.Write(", ");
                            else Console.Write("  ");
                            Console.Write(pGrid[x, reverseY, reverseZ][element]);
                        }

                        if (pGrid[x, y, reverseZ].Count < 3)
                        {
                            for (int element = 0; element < 3 - pGrid[x, reverseY, reverseZ].Count; element++)
                            {
                                Console.Write("   ");

                            }
                        }
                        Console.Write(" ]");
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("=================================================================");
            }
        }


        /// <summary>
        /// Copy the values of grid 1 to grid 0. Mostly used for resetting the level
        /// </summary>
        /// <param name="pGrid0">The grid that will be changed</param>
        /// <param name="pGrid1">The grid that will contain the values you want</param>
        /// <returns></returns>
        public void CopyGrid(List<int>[,,] pGrid0, List<int>[,,] pGrid1)
        {
            try
            {
                for (int z = 0; z < pGrid0.GetLength(2); z++)
                {
                    for (int y = 0; y < pGrid0.GetLength(1); y++)
                    {
                        for (int x = 0; x < pGrid0.GetLength(0); x++)
                        {
                            pGrid0[x, y, z] = pGrid1[x, y, z];
                        }
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



    }
}
