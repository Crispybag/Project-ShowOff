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
        public List<GameObject>[,,] roomArray;
        public List<GameObject>[,,] roomStatic;

        public List<GameObject> gameObjects;
        public List<Player> players;
        public List<SpawnPoint> spawnPoints;
        public Dictionary<int, GameObject> InteractableGameobjects = new Dictionary<int, GameObject>();
        public int minX = 0, minY = 0, minZ = 0;
        public int currentDialogue;
        public bool isReloading = false;
        public string levelFile;
        #region initialization
        public GameRoom(TCPGameServer pServer, int roomWidth, int roomHeight, int roomLength) : base(pServer)
        {
            //set up rooms
            roomArray = new List<GameObject>[roomWidth, roomHeight, roomLength];
            roomStatic = new List<GameObject>[roomWidth, roomHeight, roomLength];

            //initialise every list in list array
            initializeAllLists();

            //set up miscellaneous lists
            players = new List<Player>();
            spawnPoints = new List<SpawnPoint>();
            gameObjects = new List<GameObject>();

        }

        /// <summary>
        /// (Leo) Makes a list for every list in the txt
        /// </summary>
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
                        roomArray[x, y, z] = new List<GameObject>();
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
                        roomStatic[x, y, z] = new List<GameObject>();
                    }

                }
            }
        }

        #endregion

        #region level data loading

        /// <summary>
        /// (Leo) Loads Level
        /// </summary>
        /// 
        public void LoadLevel(string filePath)
        {
            GenerateGridFromText(filePath);
            foreach (TCPMessageChannel pListener in _users)
            {
                SpawnPlayer(pListener, _server.allConnectedUsers[pListener].GetPlayerIndex());

                foreach (Player player in players)
                {
                    player.SendConfMove();
                }    
            }
        }

        /// <summary>
        /// Generates Grid
        /// </summary>
        /// <param name="filePath"></param>
        public void GenerateGridFromText(string filePath)
        {
            levelFile = filePath;
            //values to determine grid size
            //clear all lists
            gameObjects.Clear();
            players.Clear();
            spawnPoints.Clear();
            InteractableGameobjects.Clear();


            //get the file path
            List<string> lines = File.ReadAllLines(filePath).ToList();
            determineGridSize(lines, out minX, out minY, out minZ);
            initializeAllLists();
            addObjectsToGrid(lines, minX, minY, minZ);
            //PrintGrid(roomArray);
        }

        /// <summary>
        /// (Leo) Determines how big the grid should be, it also returns minX and minY to reposition all items to fit into the grid
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
            roomArray = new List<GameObject>[maxX + 1 - minX, maxY + 1 - minY, maxZ + 1 - minZ];
        }

        /// <summary>
        /// (Leo) Creates a list from the text file
        /// </summary>
        /// <returns></returns>
        private List<List<int>> createLists(string[] pRawInformation, out List<string> informationData)
        {

            informationData = pRawInformation.ToList<string>();

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
                    informationData.Remove(info);
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
                        informationData.Remove(info);
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
                    informationData.Remove(info);
                    isAddingToList = true;
                }
            }

            return lists;

        }

        /// <summary>
        /// (Leo) Add all objects to the grid based on the information
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

                List<string> informationData = new List<string>();

                List<List<int>> informationLists = createLists(rawInformation, out informationData);


            
                switch((int)float.Parse(rawInformation[0]))
                {
                    case (1):
                        //TODO: this might be an issue as we need to link a TCPChannel to a player, it is probably better to link spawn points and spawn those into the level
                        break;

                    case (2):
                        //coordinates are the information values + the minimum X to put the most left/most bottom/ most forward object in the (0,0,0) spot
                        Wall wall = new Wall(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ );
                        break;

                    case (3):
                        SpawnPoint spawnPoint = new SpawnPoint(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, (int)float.Parse(rawInformation[7]));
                        break;

                    case (4):
                        importLever(informationLists, rawInformation, minX, minY, minZ);
                        break;

                    case (5):
                        importPressurePlate(informationLists, rawInformation, minX, minY, minZ);
                        break;

                    case (6):
                        importDoor(informationLists, rawInformation, minX, minY, minZ);
                        break;

                    case (7):
                        Box box = new Box(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, (int)float.Parse(rawInformation[7]));
                        break;

                    case (8):
                        importButton(informationLists,rawInformation, informationData, minX, minY, minZ);
                        break;

                    case (9):
                        importElevator(informationLists, rawInformation, minX ,minY,minZ);
                        break;

                    case (10):
                        Slope slope = new Slope(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, (int)float.Parse(rawInformation[7]));
                        break;
                    case (12):
                        importCrack(informationLists, rawInformation, minX, minY, minZ);
                        break;
                    case (13):
                        importAirChannel(informationLists, rawInformation, minX, minY, minZ);
                        break;
                    case (14):
                        LevelLoader levelLoader = new LevelLoader(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, rawInformation[7]);
                        break;

                    case (15):
                        importDialogue(informationLists, rawInformation, minX, minY, minZ);
                        break;
                    case (16):
                        importWater(informationLists, rawInformation, minX, minY, minZ);
                        break;
                }

            }
        }

        /// <summary>
        /// (Ezra) Imports cracks from txt
        /// </summary>
        private void importCrack(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            Crack crack = new Crack(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, (int)float.Parse(rawInformation[7]));
            Logging.LogInfo("GameRoom.cs: Added crack!", Logging.debugState.DETAILED);
            InteractableGameobjects.Add(crack.ID, crack);
            try
            {
                foreach (int index in informationLists[0])
                {
                    Logging.LogInfo("GameRoom.cs: Added door to crack!", Logging.debugState.DETAILED);
                    crack.doors.Add(index);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about crack", Logging.debugState.DETAILED);
            }
        }

        /// <summary>
        /// (Ezra) Imports water from txt
        /// </summary>
        private void importWater(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            try
            {
                WaterPool waterPool = new WaterPool(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, int.Parse(rawInformation[7]), GameObject.CollInteractType.PASS);
                InteractableGameobjects.Add(waterPool.ID ,waterPool);
                for (int i = 0; i < informationLists[0].Count / 3; i++)
                {
                    try
                    {
                        EmptyGameObject empty = new EmptyGameObject(this, informationLists[0][3 * i] - minX, informationLists[0][3 * i + 1] - minY, informationLists[0][3 * i + 2] - minZ);
                        waterPool.waterLevelPositions.Add(empty);
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong when trying to initialize empty");
                    }
                }
                for (int i = 0; i < informationLists[1].Count / 3; i++)
                {
                    try
                    {
                        Water water = new Water(this, informationLists[1][3 * i] - minX, informationLists[1][3 * i + 1] - minY, informationLists[1][3 * i + 2] - minZ);
                        waterPool.waterBlocks.Add(water);
                        Console.WriteLine("Added an water on position: " + (water.x() - minX) + "," + (water.y() - minY) + "," + (water.z() - minZ));
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong when trying to initialize water");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong with water");
            }
        }

        /// <summary>
        /// (Ezra) Imports dialogue from txt
        /// </summary>
        private void importDialogue(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            Dialogue dia = new Dialogue(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, int.Parse(rawInformation[7]));
            InteractableGameobjects.Add(dia.ID, dia);
            Logging.LogInfo("GameRoom.cs: Added dialogue to room!", Logging.debugState.DETAILED);
        }

        /// <summary>
        /// (Ezra) Import pressure plates from txt
        /// </summary>
        private void importPressurePlate(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            PressurePlate pressurePlate = new PressurePlate(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, int.Parse(rawInformation[7]), false);
            InteractableGameobjects.Add(pressurePlate.ID, pressurePlate);
            try
            {
                foreach (int index in informationLists[0])
                {
                    Logging.LogInfo("GameRoom.cs: Added door to pressure plate!", Logging.debugState.DETAILED);
                    pressurePlate.doors.Add(index);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about pressure plate", Logging.debugState.DETAILED);
            }
        }
        /// <summary>
        /// (Ezra) Import lever plates from txt
        /// </summary>
        private void importLever(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            Lever lever = new Lever(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, int.Parse(rawInformation[7]), false);
            InteractableGameobjects.Add(lever.ID, lever);
            try
            {
                foreach (int index in informationLists[0])
                {
                    Logging.LogInfo("GameRoom.cs: Added door to lever!", Logging.debugState.DETAILED);
                    lever.doors.Add(index);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about lever", Logging.debugState.DETAILED);
            }
        }
        /// <summary>
        /// (Ezra) Import door plates from txt
        /// </summary>
        private void importDoor(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            Door door = new Door(this, int.Parse(rawInformation[1]) - minX, int.Parse(rawInformation[2]) - minY, int.Parse(rawInformation[3]) - minZ, int.Parse(rawInformation[7]), true);
            InteractableGameobjects.Add(door.ID, door);
            try
            {
                foreach (int index in informationLists[0])
                {
                    Logging.LogInfo("GameRoom.cs: Added actuator to door!", Logging.debugState.DETAILED);
                    door.actuators.Add(index);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about door", Logging.debugState.DETAILED);
            }
        }
        /// <summary>
        /// (Ezra) Import button plates from txt
        /// </summary>
        private void importButton(List<List<int>> informationLists ,string[] rawInformation, List<string> informationData, int minX, int minY, int minZ)
        {
            Button button = new Button(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, int.Parse(rawInformation[7]));
            InteractableGameobjects.Add(button.ID, button);
            Logging.LogInfo("GameRoom.cs: Added button!", Logging.debugState.DETAILED);
            switch (int.Parse(informationData[8]))
            {
                case (0):
                    button.currentDirection = Button.Direction.DOWN;
                    break;
                case (1):
                    button.currentDirection = Button.Direction.UP;
                    break;
                default:
                    Logging.LogInfo("GameRoom.cs: Cant handle button direction!", Logging.debugState.DETAILED);
                    break;
            }
            try
            {
                foreach (int index in informationLists[0])
                {
                    Logging.LogInfo("GameRoom.cs: Added door to button!", Logging.debugState.DETAILED);
                    button.elevators.Add(index);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about button", Logging.debugState.DETAILED);
            }
        }
        /// <summary>
        /// (Ezra) Import elevator plates from txt
        /// </summary>
        private void importElevator(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            Elevator elevator = new Elevator(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, int.Parse(rawInformation[7]));
            InteractableGameobjects.Add(elevator.ID, elevator);
            Console.WriteLine("Added an elevator on positions: " + elevator.x() + "," + elevator.y() + "," + elevator.z());
            try
            {
                for (int i = 0; i < informationLists[0].Count / 3; i++)
                {
                    EmptyGameObject empty = new EmptyGameObject(this, informationLists[0][3 * i] - minX, informationLists[0][3 * i + 1] - minY, informationLists[0][3 * i + 2] - minZ);
                    Console.WriteLine("Added an empty on positions: " + (empty.x() - minX) + "," + (empty.y() - minY) + "," + (empty.z() - minZ));
                    elevator.points.Add(i, empty);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// (Leo) Import Airchannel from text
        /// </summary>
        private void importAirChannel(List<List<int>> informationLists, string[] rawInformation, int minX, int minY, int minZ)
        {
            Logging.LogInfo(rawInformation[9], Logging.debugState.DETAILED);
            AirChannel airChannel = new AirChannel(this, (int)float.Parse(rawInformation[1]) - minX, (int)float.Parse(rawInformation[2]) - minY, (int)float.Parse(rawInformation[3]) - minZ, (int)float.Parse(rawInformation[7]), (int)float.Parse(rawInformation[8]), (int)float.Parse(rawInformation[9]), int.Parse(rawInformation[10]) );
            InteractableGameobjects.Add(airChannel.ID, airChannel);
            try
            {
                foreach (int index in informationLists[0])
                {
                    Logging.LogInfo("GameRoom.cs: Added actuator to airChannel!", Logging.debugState.DETAILED);
                    airChannel.actuators.Add(index);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about door", Logging.debugState.DETAILED);
            }

        }

        #endregion

        #region network handling
        //=========================================================================================
        //                                 > Network Handling <
        //=========================================================================================

        //Receives a package and determine how to open the package
        protected override void handleNetworkMessage(ASerializable pMessage, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a package! Trying to handle", Logging.debugState.SPAM);
            if (pMessage is ReqKeyDown){handleReqKeyDown(pMessage as ReqKeyDown, pSender);}
            if (pMessage is ReqKeyUp){handleReqKeyUp(pMessage as ReqKeyUp, pSender);}
            if (pMessage is ReqProgressDialogue){ handleReqProgressDialogue(pSender);}
            if (pMessage is ReqResetLevel) { handleReqResetLevel(pMessage as ReqResetLevel, pSender); }
            if (pMessage is ReqJoinRoom) { handleRoomRequest(pMessage as ReqJoinRoom, pSender); }
        }

        private void handleRoomRequest(ReqJoinRoom pMessage, TCPMessageChannel pSender)
        {
            if ((int)pMessage.room == 0)
            {
                Logging.LogInfo("Moving client to login room", Logging.debugState.DETAILED);
                ConfJoinRoom confirmLoginRoom = new ConfJoinRoom();
                confirmLoginRoom.room = 0;
                pSender.SendMessage(confirmLoginRoom);
                _server.availableRooms["Login"].AddMember(pSender);
                removeAndCloseMember(pSender);
            }
            else
            {
                Console.WriteLine("We didnt implement any other rooms switching yet in gameroom");
            }
        }

        private void handleReqProgressDialogue(TCPMessageChannel pSender)
        {
            ConfProgressDialogue progressDialogue = new ConfProgressDialogue();
            progressDialogue.ID = currentDialogue;
            sendToUser(progressDialogue, pSender);
        }

        /// <summary>
        /// (Leo) Handles request key down
        /// </summary>
        private void handleReqKeyDown(ReqKeyDown pKeyDown, TCPMessageChannel pSender)
        {
            Logging.LogInfo("Received a HandleReqKeyDown Package", Logging.debugState.DETAILED);
            
            //search which player should be influenced by this package send
            foreach (Player player in players)
            {
                if (pSender == player.getClient())
                {
                    //goes to player for further processing of the package
                    player.addInput(pKeyDown.keyInput, pKeyDown.rotation);
                }
            }
        }

        /// <summary>
        /// (Leo) Handles request key up
        /// </summary>
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

        private void handleReqResetLevel(ReqResetLevel pResetLevel, TCPMessageChannel pSender)
        {
            foreach (Player player in players)
            {
                if (pSender == player.getClient())
                {
                    player.SetResetStatus(pResetLevel.wantsReset);
                }
            }
        }

        private void sendLevelReset()
        {
            ConfReloadScene reloadScene = new ConfReloadScene();
            sendToAll(reloadScene);
        }
        #endregion

        #region resetting

        /// <summary>
        /// (Leo) Teleports player to certain coordinates
        /// </summary>
        protected void SetPlayerCoord(TCPMessageChannel pListener, int pX, int pY, int pZ, int pPlayerIndex)
        {
            foreach(Player player in players)
            {
                if (pPlayerIndex == player.GetPlayerIndex())
                {
                    players.Remove(player);
                }
            }
            players.Add(new Player(this, pListener, pX, pY, pZ, pPlayerIndex));
        }
        /// <summary>
        /// (Leo) Handles player spawning
        /// </summary>
        /// <param name="pListener"></param>
        protected void SpawnPlayer(TCPMessageChannel pListener, int pClientID)
        {
            //checks if player 1 is connected
            bool player1Connected = false;

            //look for a player with player index 0 (player 1)
            foreach (Player player in players)
            {
                if (player.GetPlayerIndex() == 0) player1Connected = true;
            }

            //spawn player 1 if player 1 is not yet connected
            if (pClientID == 0)
            {
                try
                {
                    setSpawnPoint(pListener, 0);
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
                    setSpawnPoint(pListener, 1);
                }
                catch
                {
                    Logging.LogInfo("AAAAAAAAAAAAAAAAAAAAAAA something went wrong lol (in gametestroom0 adding member)", Logging.debugState.SIMPLE);
                }

                //SetPlayerCoord(pListener, 9, 0, 0, 1);
            }
        }

        /// <summary>
        /// (Leo) Determines and sets a player spawnpoint
        /// </summary>
        /// <param name="pListener"></param>
        /// <param name="playerIndex"></param>
        private void setSpawnPoint(TCPMessageChannel pListener, int playerIndex)
        {
            //spawn player at spawn point if there is one
            List<GameObject> _spawnPoints = OnCoordinatesFindGameObjectOfType(3);
            SpawnPoint _spawnPoint = null;

            foreach (GameObject obj in _spawnPoints)
            {
                SpawnPoint smiley = obj as SpawnPoint;

                //set spawnpoint to smiley if spawnpoint has been found
                if (smiley.spawnIndex == playerIndex) _spawnPoint = smiley;
            }

            //set the player coordinates to the spawn point coordinates
            if (null != _spawnPoint) SetPlayerCoord(pListener, _spawnPoint.x(), _spawnPoint.y(), _spawnPoint.z(), playerIndex);
        }

        /// <summary>
        /// (Leo) Resets room to initial state
        /// </summary>
        public void ResetRoom()
        {
            //for debug purposes print grid
            Logging.LogInfo("Current Grid");
            //PrintGrid(roomArray);

            Logging.LogInfo("\n\nSaved Grid");
            //PrintGrid(roomStatic);

            //copy static grid to room grid
            CopyGrid(roomArray, roomStatic);

            //reset player positions
            RespawnPlayer();

            //probably need to handle lever states or whatever aswell
        }

        /// <summary>
        /// (Leo) Respawns player
        /// </summary>
        public void RespawnPlayer()
        {
            try
            {
                //for each player teleport to corresponding respawn point
                for (int i = 0; i < players.Count; i++)
                {
                    Logging.LogInfo("teleporting to:  ( " + spawnPoints[i].x() + ", " + spawnPoints[i].y() + ")");
                    players[i].MovePosition(spawnPoints[i].x(), spawnPoints[i].y(), spawnPoints[1].z());
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
            List<Player> playersToRemove = new List<Player>();
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i] == pListener)
                {
                    playersToRemove.Add(players[i]);
                }
            }

            foreach (Player pPlayer in playersToRemove)
            {
                players.Remove(pPlayer);
            }

            playersToRemove.Clear();
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

            foreach(Player player in players)
            {
                if (!player.wantsReset) break;
                isReloading = true;
            }

            if (isReloading)
            {

                sendLevelReset();
                foreach (TCPMessageChannel pListener in _users)
                {
                    sendConfPlayer(pListener);
                }

                LoadLevel(levelFile);
                isReloading = false;
            }
        }
        #endregion

        #region grid tools

        /// <summary>
        /// (Leo) Finds a single object of given type, it only works with singletons at the moment, so if this needs an upgrade for other objects, notify me
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
        /// (Leo) Checks if the coordinates have a certain value
        /// </summary>
        /// <param name="pX"> x-coordinate</param>
        /// <param name="pY"> y-coordinate</param>
        /// <param name="pValue"> value on coordinate </param>
        /// <returns>true if the coordinates contain the value</returns>
        public bool OnCoordinatesContain(int[] pPos, int pValue)
        {
            try
            {
                foreach (GameObject value in roomArray[pPos[0], pPos[1], pPos[2]])
                {
                    if (value.objectIndex == pValue) return true;
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
        /// (Leo) Checks if the coordinates have a certain value
        /// </summary>
        /// <param name="pX"> x-coordinate</param>
        /// <param name="pY"> y-coordinate</param>
        /// <param name="pValue"> value on coordinate </param>
        /// <returns>true if the coordinates contain the value</returns>
        public bool OnCoordinatesContain(int pX, int pY, int pZ, int pValue)
        {
            try
            {
                foreach (GameObject value in roomArray[pX, pY, pZ])
                {
                    if (value.objectIndex == pValue) return true;
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
        /// (Leo) Checks if coordinates[pX, pY] is empty
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
        /// (Leo) Checks if coordinates[pX, pY] is empty
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
        /// (Leo) checks if there is a gameobject at coordinates that is solid
        /// </summary>
        /// <param name="pPos"></param>
        /// <returns></returns>
        public bool OnCoordinatesCanMove(int[] pPos)
        {
            try
            {
                if (roomArray[pPos[0], pPos[1], pPos[2]].Count != 0)
                {
                    foreach (GameObject obj in roomArray[pPos[0], pPos[1], pPos[2]])
                    {
                        if (obj.collState == GameObject.CollInteractType.SOLID)
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                Logging.LogInfo("CoordinatesCanMove got a pPos value that was not 3 long, might want to recheck that", Logging.debugState.SIMPLE);

                return false;
            }
        }

        /// <summary>
        /// (Leo)checks if there is a gameobject at coordinates that is solid
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pZ"></param>
        /// <returns></returns>
        public bool OnCoordinatesCanMove(int pX, int pY, int pZ)
        {
            try
            {
                if (roomArray[pX, pY, pZ].Count != 0)
                {
                    foreach (GameObject obj in roomArray[pX, pY, pZ])
                    {
                        if (obj.collState == GameObject.CollInteractType.SOLID)
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                Logging.LogInfo("CoordinatesCanMove got a pPos value that was not 3 long, might want to recheck that", Logging.debugState.SIMPLE);

                return false;
            }
        }

        /// <summary>
        /// (Leo) Quick tool function to remove all values from given value also gameobjects
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesRemove(int[] pPos, int pValue)
        {
            List<GameObject> thingToRemove = new List<GameObject>();

            Logging.LogInfo("GameRoom.cs: Trying to remove a object at : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + " with the value of: " + pValue, Logging.debugState.DETAILED);
            try
            {
                foreach (GameObject obj in roomArray[pPos[0], pPos[1], pPos[2]])
                {

                    if (obj.objectIndex == pValue)
                    {
                        thingToRemove.Add(obj);
                    }
                }
                foreach (GameObject removedValue in thingToRemove)
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
        /// (Leo) Quick tool function to remove all values from given value
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesRemove(int pX, int pY, int pZ, int pValue)
        {
            List<GameObject> thingToRemove = new List<GameObject>();

            Logging.LogInfo("GameRoom.cs: Trying to remove a object at : " + pX + "," + pY + "," + pZ + " with the value of: " + pValue, Logging.debugState.DETAILED);
            try
            {
                foreach (GameObject value in roomArray[pX, pY, pZ])
                {

                    if (value.objectIndex == pValue)
                    {
                        thingToRemove.Add(value);
                    }
                }
                foreach (GameObject removedValue in thingToRemove)
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
        /// (Leo) Adds a new objects (int) to given position
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesAdd(int[] pPos, GameObject pValue)
        {
            pValue.MovePosition(pPos);
            roomArray[pPos[0], pPos[1], pPos[2]].Add(pValue);
        }

        /// <summary>
        /// Adds a new objects (int) to given position
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pValue">value</param>
        public void OnCoordinatesAdd(int pX, int pY, int pZ, GameObject pValue)
        {
            pValue.MovePosition(pX, pY, pZ);
            roomArray[pX, pY, pZ].Add(pValue);
        }


        /// <summary>
        /// (Leo) Gets all objects from the location
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        public List<GameObject> OnCoordinatesGetIndexes(int[] pPos)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (GameObject value in roomArray[pPos[0], pPos[1], pPos[2]])
            {
                objects.Add(value);
            }
            return objects;
        }

        /// <summary>
        /// (Leo) Gets all objects from the location
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        public List<GameObject> OnCoordinatesGetIndexes(int pX, int pY, int pZ)
        {
            List<GameObject> objects = new List<GameObject>();
            foreach (GameObject value in roomArray[pX, pY, pZ])
            {
                objects.Add(value);
            }
            return objects;
        }

        /// <summary>
        /// (Leo) Get game object of index on specific coordinate
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
                    if (obj.x() == pPos[0] && obj.y() == pPos[1] && obj.z() == pPos[2] && obj.objectIndex == index)
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
        /// (Leo) Get game object of index on specific coordinate
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
                    if (obj.x() == pX && obj.y() == pY && obj.z() == pZ && obj.objectIndex == index)
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
        /// (Leo) Get all game objects of index(if specified) on specific coordinate
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pIndex">index type of the gameobject</param>
        /// <returns></returns>
        public List<GameObject> OnCoordinatesGetGameObjects(int[] pPos, int pIndex = -1)
        {
            List<GameObject> objectList = new List<GameObject>();
            foreach (GameObject gObj in roomArray[pPos[0], pPos[1], pPos[2]])
            {
                objectList.Add(gObj);
            }

            return objectList;
        }

        /// <summary>
        /// (Leo) Get all game objects of index(if specified) on specific coordinate
        /// </summary>
        /// <param name="pX">x-coordinate</param>
        /// <param name="pY">y-coordinate</param>
        /// <param name="pIndex">index type of the gameobject</param>
        /// <returns></returns>
        public List<GameObject> OnCoordinatesGetGameObjects(int pX, int pY, int pZ, int pIndex = -1)
        {
            List<GameObject> objectList = new List<GameObject>();
            foreach (GameObject gObj in roomArray[pX,pY,pZ])
            {
                objectList.Add(gObj);
            }
            return objectList;
        }


        /// <summary>
        /// (Leo) Tool that prints the entire grid in the console, for debugging purposes (not yet tested with list printing)
        /// </summary>
        /// <param name="pGrid"></param>
        ///<returns></returns>
        public void PrintGrid(List<GameObject>[,,] pGrid)
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
                            Console.Write(pGrid[x, reverseY, reverseZ][element].objectIndex);
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
        /// (Leo) Copy the values of grid 1 to grid 0. Mostly used for resetting the level
        /// </summary>
        /// <param name="pGrid0">The grid that will be changed</param>
        /// <param name="pGrid1">The grid that will contain the values you want</param>
        /// <returns></returns>
        public void CopyGrid(List<GameObject>[,,] pGrid0, List<GameObject>[,,] pGrid1)
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
