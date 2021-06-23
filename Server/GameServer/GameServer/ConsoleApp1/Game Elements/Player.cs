using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using sharedAngy;

namespace Server
{
    public class Player : GameObject
    {
        private TCPMessageChannel _client;
        private int[] walkDirection;

        private int timer = 0;

        private int[] orientation;
        private bool _hasBox;
        private int currentBoxID;

        public int playerIndex;
        public Box currentBox;
        private int[,,] position;

        private bool isFalling = false;

        private int callLoopPrevent;
        public bool wantsReset;
        private int cameraRotation;
        private bool endsInAirChannel;
        private int refreshSpeed = 2;

        //standard stuff, along with quick coordinates
        public enum PlayerType
        {
            ALEX = 0,
            NUC = 1
        }

        public PlayerType playerType;

        private float calls;
        private string directionCommands;

        public Player(GameRoom pRoom, TCPMessageChannel pClient, int pX = 0, int pY = 0, int pZ = 0, int pPlayerIndex = 0, PlayerType pPlayerType = PlayerType.NUC) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            orientation = new int[2] { 0, 1 };
            walkDirection = new int[3];
            room = pRoom;
            _client = pClient;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 1;
            playerIndex = pPlayerIndex;
            if (playerIndex == 0) { playerType = PlayerType.NUC; }
            else { playerType = PlayerType.ALEX; }
        }

        #region input

        /// <summary>
        /// (Leo) adds a direction input for the server to remember that the player can move in a certain direction
        /// </summary>
        public void addInput(ReqKeyDown.KeyType lastInput, int rotation)
        {

            switch (lastInput)
            {
                //up
                case (ReqKeyDown.KeyType.UP):
                    //for each of those, add a direction vector to their movement and immediately try to change position as well to minimise latency.
                    changeWalkDirection(0, 0, 1, rotation);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;

                //down
                case (ReqKeyDown.KeyType.DOWN):
                    changeWalkDirection(0, 0, -1, rotation);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;

                //left
                case (ReqKeyDown.KeyType.LEFT):
                    changeWalkDirection(-1, 0, 0, rotation);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;


                //right
                case (ReqKeyDown.KeyType.RIGHT):
                    changeWalkDirection(1, 0, 0, rotation);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;

                //interaction
                case (ReqKeyDown.KeyType.INTERACTION):
                    Logging.LogInfo("Received an interaction key request", Logging.debugState.SPAM);
                    handleInteraction();
                    //do something
                    break;

                //if we dont handle incomming keytype
                default:
                    Logging.LogInfo("Received an incomming keypacket which could not be handled within player", Logging.debugState.SIMPLE);
                    break;

            }



        }

        /// <summary>
        /// (Leo) handle the release of the input where you remove the input from the list and cancel the direction if it is the direction the player is currently moving in
        /// </summary>
        public void removeInput(ReqKeyUp.KeyType lastInput)
        {
            switch (lastInput)
            {
                case (ReqKeyUp.KeyType.UP):
                    tryCancelDirection(0, 0, 1);
                    break;

                case (ReqKeyUp.KeyType.DOWN):
                    tryCancelDirection(0, 0, -1);
                    break;

                case (ReqKeyUp.KeyType.LEFT):
                    tryCancelDirection(-1, 0, 0);
                    break;

                case (ReqKeyUp.KeyType.RIGHT):
                    tryCancelDirection(1, 0, 0);
                    break;




            }

        }

        public void SetResetStatus(bool pReset)
        {
            wantsReset = pReset;
        }


        //change the walk direction, the direction in which the player wants to move
        private void changeWalkDirection(int pX, int pY, int pZ, int rotation)
        {
            switch(rotation)
            {
                case (90):
                    walkDirection = new int[3] { pZ, pY, -pX };
                    break;
                case (180):
                    walkDirection = new int[3] { -pX, pY, -pZ };
                    break;
                case (270):
                    walkDirection = new int[3] { -pZ, pY, pX };
                    break;
                default:
                    walkDirection = new int[3] { pX, pY, pZ };
                    break;
            }
            cameraRotation = rotation;
        }

        /// <summary>
        /// (Leo) Cancel the movement direction and then player stop :)
        /// </summary>
        private void tryCancelDirection(int pX, int pY, int pZ)
        {
            switch (cameraRotation)
            {

                case (90):
                    if (walkDirection[0] == pZ && walkDirection[1] == pY && walkDirection[2] == -pX)
                    {
                        walkDirection[0] = 0; walkDirection[1] = 0; walkDirection[2] = 0;

                    }
                    break;
                case (180):
                    if (walkDirection[0] == -pX && walkDirection[1] == pY && walkDirection[2] == -pZ)
                    {
                        walkDirection[0] = 0; walkDirection[1] = 0; walkDirection[2] = 0;

                    }
                    break;
                case (270):
                    if (walkDirection[0] == -pZ && walkDirection[1] == pY && walkDirection[2] == pX)
                    {
                        walkDirection[0] = 0; walkDirection[1] = 0; walkDirection[2] = 0;

                    }
                    break;

                default:
                    if (walkDirection[0] == pX && walkDirection[1] == pY && walkDirection[2] == pZ)
                    {
                        walkDirection[0] = 0; walkDirection[1] = 0; walkDirection[2] = 0;

                    }
                    break;
            }
            
        }

        /// <summary>
        /// (Ezra) Handles the interaction button, check if there are any interactionables around, and interacts with them
        /// </summary>
        private void handleInteraction()
        {

            try
            {

                if (null == currentBox)
                {
                    //for lever
                    if (room.OnCoordinatesContain(OneInFront(), 4))
                    {
                        //stun timer
                        calls += 3;
                        sendActuatorToggle(OneInFront());
                    }

                    //for button
                    if (room.OnCoordinatesContain(OneInFront(), 8))
                    {
                        //stun timer
                        calls += 3;
                        sendActuatorToggle(OneInFront());
                    }

                    //for crack
                    if (room.OnCoordinatesContain(OneInFront(), 12))
                    {
                        //stun timer
                        calls += 3;
                        sendActuatorToggle(OneInFront());
                    }

                    //for box
                    else if (room.OnCoordinatesContain(OneInFront(), 7))
                    {
                        //stun timer
                        calls += 3;
                        if (playerType == PlayerType.NUC)sendPickUpBox(OneInFront());
                    }
                }
                else
                {
                    handleBox();
                }
            }
            catch (Exception e)
            {
                Logging.LogInfo("Player.cs: " + e.Message, Logging.debugState.DETAILED);
            }
        }

        private void handleBox()
        {
            //check if box can be placed (at position + orientation) if its empty
            if (room.OnCoordinatesEmpty(OneInFront()))
            {
                //place box
                try
                {
                    currentBox.MovePosition(OneInFront());
                    currentBox.CheckGrounded();
                    currentBox.sendBoxPackage(false);

                    handleWaterInteractionBox();

                    sendBoxPackage(currentBox, false);
                    currentBox = null;
                }
                catch { Logging.LogInfo("One In Front does not return an array that is 3 in length", Logging.debugState.SIMPLE); }

            }
            //if its not empty, there are a few exceptions like the pressure plate
            else
            {
                //Console.WriteLine("Box is checking in front!");
                List<GameObject> index = room.OnCoordinatesGetIndexes(OneInFront());
                foreach (GameObject item in index)
                {
                    //5 is pressure plate, 16 is water
                    if (item.objectIndex != 5 && item.objectIndex != 16 && item.objectIndex != 13)
                    {
                        //if its anything else, return out of the method
                        return;
                    }
                }
                //Console.WriteLine("In front is empty for box! but with interactable");
                currentBox.MovePosition(OneInFront());
                currentBox.sendBoxPackage(false);

                handleWaterInteractionBox();

                sendBoxPackage(currentBox,  false);
                currentBox = null;
            }


        }
        public void handleWaterInteractionBox()
        {
            List<GameObject> objects = room.OnCoordinatesGetGameObjects(currentBox.x(), currentBox.y(), currentBox.z());
            foreach (GameObject obj in objects)
            {
                if (obj is Water)
                {
                    if ((obj as Water).CanPushBox(obj.x(), obj.y(), obj.z()))
                    {
                        currentBox.MovePosition((obj as Water).PushBox(obj.x(), obj.y(), obj.z()));
                        if ((currentBox as Box).tileContainsWater((currentBox as Box).ID))
                        {
                            handleWaterInteractionBox();
                        }
                        break;
                    }
                }
            }
            //room.PrintGrid(room.roomArray);
        }

        private void sendBoxPackage(GameObject box, bool pIsPickedUp)
        {
            try
            {
                if (pIsPickedUp)
                {
                    box.SetState(CollInteractType.PASS);
                }
                else
                {
                    box.SetState(CollInteractType.SOLID);
                }
                BoxInfo boxInfo = new BoxInfo();
                boxInfo.ID = (box as Box).ID;
                boxInfo.isPickedUp = pIsPickedUp;
                boxInfo.posX = currentBox.x() + room.minX;
                boxInfo.posY = currentBox.y() + room.minY;
                boxInfo.posZ = currentBox.z() + room.minZ;
                room.sendToAll(boxInfo);
            }
            catch
            {
                Logging.LogInfo("Something went wrong when trying to adjust the box", Logging.debugState.SIMPLE);
            }
        }

        private void sendBoxPackage(GameObject box, int pX, int pY, int pZ, bool pIsPickedUp)
        {
            try
            {
                BoxInfo boxInf = new BoxInfo();
                boxInf.ID = (box as Box).ID;
                boxInf.isPickedUp = pIsPickedUp;
                boxInf.posX = currentBox.x() + room.minX;
                boxInf.posY = currentBox.y() + room.minY;
                boxInf.posZ = currentBox.z() + room.minZ;
                room.sendToAll(boxInf);
            }
            catch
            {
                Logging.LogInfo("Something went wrong when trying to adjust the box", Logging.debugState.SIMPLE);
            }
        }

        #endregion

        #region movement checks
        //=========================================================================================
        //                                    > Movement Check <
        //=========================================================================================

        /// <summary>
        /// (Leo) check if player is being blocked
        /// </summary>
        /// <param name="position">position player tries to move to</param>
        /// <returns></returns>
        private bool isBlockedByObject(int[] position)
        {
            List<GameObject> gameObjects = room.OnCoordinatesGetGameObjects(position);

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.collState == CollInteractType.SOLID) return true;
                if(gameObject.objectIndex == 15)
                {
                    startDialogue(gameObject, position);
                    return false;
                }
            }

            //checks if player can drop down (no water)
            for (int i = 1; i < 15; i++)
            {
                if(position[1] - i < 0)
                {
                    return false;
                }
                List<GameObject> gameObjectsDown = room.OnCoordinatesGetGameObjects(position[0], position[1] - i, position[2]);
                
                foreach (GameObject obj in gameObjectsDown)
                {
                    if (obj.collState == CollInteractType.SOLID)
                    {
                        if (!(obj is Water))
                        {
                            return false;
                        }
                        else return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// (Leo) handle when slope 0 is being hit
        /// </summary>
        private void handleS0Hit(int[] pPosition)
        {
            //check if coordinates have a slope object
            if (room.OnCoordinatesGetGameObject(pPosition, 10) is Slope)
            {
                //get the slope as game object
                Slope pSlope = room.OnCoordinatesGetGameObject(pPosition, 10) as Slope;
                int[] directionVec;
                if (calls == 1)directionVec = new int[3] { pSlope.MoveOnSlope(pPosition)[0] - pPosition[0], pSlope.MoveOnSlope(pPosition)[1] - pPosition[1], pSlope.MoveOnSlope(pPosition)[2] - pPosition[2]};
                else { directionVec = new int[3] { pSlope.MoveOnSlope(pPosition)[0] - pPosition[0], pSlope.MoveOnSlope(pPosition)[1] - pPosition[1], pSlope.MoveOnSlope(pPosition)[2] - pPosition[2]}; }
                //check if the player can move on the slope and move on it when the player can move on the slope
                if (pSlope.CanMoveOnSlope(pPosition, orientation) == 0)
                {
                    addMoveDirection(directionVec[0], directionVec[1], directionVec[2]);
                    addMoveDirection(orientation[0], 0, orientation[1]);
                    MovePosition(pSlope.MoveOnSlope(pPosition));
                }
                
                //if the slope is blocked by anything
                else if (pSlope.CanMoveOnSlope(pPosition, orientation) == 1)
                {
                    addMoveDirection(directionVec[0], directionVec[1], directionVec[2]);
                    checkSpecialCollision(pSlope.MoveOnSlope(pPosition));

                }

                //if player walks in wrong angle or slope goes off the map
                else
                {
                    calls = 0;
                    directionCommands = "";
                }

            }

            //else it could be the s2 position so check that as well here
            else if (room.OnCoordinatesGetGameObject(pPosition[0] + orientation[0], pPosition[1] - 1, pPosition[2] + orientation[1], 10) is Slope)
            {
                //if that is true return the slope on that coordinate to the player
                Slope pSlope = room.OnCoordinatesGetGameObject(pPosition[0] + orientation[0], pPosition[1] - 1, pPosition[2] + orientation[1], 10) as Slope;

                int[] directionVec;
                if (calls == 1) directionVec = new int[3] { pSlope.MoveOnSlope(pPosition)[0] - pPosition[0], pSlope.MoveOnSlope(pPosition)[1] - pPosition[1], pSlope.MoveOnSlope(pPosition)[2] - pPosition[2]};
                else { directionVec = new int[3] { pSlope.MoveOnSlope(pPosition)[0] - pPosition[0], pSlope.MoveOnSlope(pPosition)[1] - pPosition[1], pSlope.MoveOnSlope(pPosition)[2] - pPosition[2] }; }
                //check with that slope whether the player can move on it
                if (pSlope.CanMoveOnSlope(pPosition, orientation) == 0)
                {
                    addMoveDirection(directionVec[0], directionVec[1], directionVec[2]);
                    addMoveDirection(orientation[0], 0, orientation[1]);

                    MovePosition(pSlope.MoveOnSlope(pPosition));
                }
                
                //if other end of slope is blocked
                else if (pSlope.CanMoveOnSlope(pPosition, orientation) == 1)
                {
                    addMoveDirection(directionVec[0], directionVec[1], directionVec[2]);
                    checkSpecialCollision(pSlope.MoveOnSlope(pPosition));
                }

                //if player is not in correct rotation
                else
                {
                    calls = 0;
                    directionCommands = "";
                }
            }
        }

        /// <summary>
        /// (Leo)handles special air channel hit event
        /// </summary>
        /// <param name="pPosition"></param>
        private void handleAirChannelHit(int [] pPosition)
        {
            if (room.OnCoordinatesGetGameObject(pPosition, 13) is AirChannel)
            {
                if (playerType == PlayerType.ALEX)
                {
                    if (callLoopPrevent == 1) 
                    { 
                        
                        addMoveDirection(orientation[0], 0, orientation[1]);
                    
                    }
                    AirChannel airChannel = room.OnCoordinatesGetGameObject(pPosition, 13) as AirChannel;
                    addMoveDirection(airChannel.direction[0], airChannel.direction[1], airChannel.direction[2]);

                    if (airChannel.CanPushPlayer(pPosition))
                    {
                        MovePosition(airChannel.PushPlayer(pPosition), true ,GetPlayerIndex());
                    }
                    else
                    {
                        checkSpecialCollision(airChannel.PushPlayer(pPosition));
                    }
                }


                else if (!room.OnCoordinatesContain(pPosition, 7))
                {
                    MoveDirection(orientation[0], 0, orientation[1]);
                    addMoveDirection(orientation[0], 0, orientation[1]);
                }
            }
        }


        /// <summary>
        /// (Leo)Contains all special interactions that need to have its own handling
        /// </summary>
        private void checkSpecialCollision(int[] pPosition)
        {
             List<GameObject> objectsOnCoord = room.OnCoordinatesGetIndexes(pPosition);

            //infinite recursive loop prevention
            callLoopPrevent++;
            if (callLoopPrevent >= 21)
            {
                Logging.LogInfo("potential infinite recursive loop in check special collision, make sure that the code runs properly or increase treshold", Logging.debugState.DETAILED);
                calls = 0;
                directionCommands = "";
                return;
            }

            foreach (GameObject index in objectsOnCoord)
            {
                switch (index.objectIndex)
                {
                    //slope hit at s0
                    case (10):
                        endsInAirChannel = false;
                        handleS0Hit(pPosition);
                        callLoopPrevent = 0;
                        break;

                        //airchannel
                    case (13):
                        endsInAirChannel = true;
                        handleAirChannelHit(pPosition);
                        callLoopPrevent = 0;
                        break;

                    default:
                        //remove all information
            
                        calls = 0;
                        callLoopPrevent = 0;
                        if (!endsInAirChannel)
                        {
                            directionCommands = "";
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// check grounded lol
        /// </summary>
        private void checkGrounded()
        {
            try
            {
                bool isGrounded = false;

                while (!isGrounded)
                {
                    int[] pPosition = new int[3] { x(), y() - 1, z() };
                    //if object is 1 below you, stop falling
                    if (isBlockedByObject(pPosition) || y() <= 0)
                    {
                        isGrounded = true;
                    }

                    //else fall 1 further down
                    else
                    {
                        ConfAnimation animation = new ConfAnimation();
                        animation.isFalling = true;
                        animation.player = GetPlayerIndex();
                        room.sendToAll(animation);
                        
                        calls++;
                        MoveDirection(0, -1, 0);
                        addMoveDirection(0, -1, 0);
                    }
                }
            }
            catch
            {
                //stop falling when it hits the bottom most coordinate
            }
        }
        //Does the check whether the player can change position or not
        public void tryPositionChange(int pX, int pY, int pZ)
        {
            //add stun timer here later, for now it is removed due to personal annoyance
            if (calls < 1)
            {
                calls = 0;
                //determine direction and set orientation
                int[] direction = { pX, pY, pZ };
                orientation[0] = pX;
                orientation[1] = pZ;
                Logging.LogInfo("Player's position is now ( " + x() + "," + y() + "," + z() + ")", Logging.debugState.SPAM);

                try
                {
                    bool objectAtLocation = isBlockedByObject(new int[3] { direction[0] + x(), direction[1] + y(), direction[2] + z() });

                    //Passes the check and can move
                    if (!objectAtLocation)
                    {
                        if (!isCrawlSpace(OneInFront()))
                        {
                            if (isCrawling)
                            {
                                ConfAnimation animation = new ConfAnimation();
                                animation.player = GetPlayerIndex();
                                animation.isCrawling = false;
                                isCrawling = false;
                                room.sendToAll(animation);
                            }
                            MoveDirection(direction);
                            addMoveDirection(direction[0], direction[1], direction[2]);

                        }
                        else if (playerType == PlayerType.ALEX)
                        {
                            //ADD CRAWLING BOOL HERE !!!!!! WWEEEWOOOWEEEWOOO
                            if (!isCrawling)
                            {
                                ConfAnimation animation = new ConfAnimation();
                                animation.player = GetPlayerIndex();
                                animation.isCrawling = true;
                                isCrawling = true;
                                room.sendToAll(animation);
                            }
                            MoveDirection(direction);
                            addMoveDirection(direction[0], direction[1], direction[2]);
                        }
                    }

                    //check objects that need to be checked
                    else
                    {
                        checkSpecialCollision(OneInFront());
                    }
                    checkGrounded();
                    //room.PrintGrid(room.roomArray);
                    //send the package to the clients
                    SendConfMove();
                }

                catch (Exception e)
                {
                    Logging.LogInfo("probably trying to move off the grid", Logging.debugState.DETAILED);
                    Logging.LogInfo(e.Message, Logging.debugState.DETAILED);
                }
            }
        }

        private bool isCrawling = false;

        private bool isCrawlSpace(int[] pPosition)
        {
            if (room.OnCoordinatesContain(pPosition[0], pPosition[1] + 1, pPosition[2], 2))
            {
                return true;
            }

            return false;
        }





        /// <summary>
        /// (Ezra) Starts new dialogue
        /// </summary>
        private void startDialogue(GameObject diaobj, int[] direction)
        {
            DialogueHitBoxes dia = diaobj as DialogueHitBoxes;
            Dialogue dialogue = dia.parentDialogue;
            ConfProgressDialogue progressDialogue = new ConfProgressDialogue();
            progressDialogue.ID = dialogue.ID;
            room.currentDialogue = dialogue.ID;
            room.sendToAll(progressDialogue);
            dialogue.DestroyDialogue();
            //room.OnCoordinatesRemove(OneInFront(), diaobj.objectIndex);
        }

        /// <summary>
        /// Adds a movement command to the string to determine player movement
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <param name="pZ"></param>
        /// <param name="pCalls"> amount of times this needs to be called</param>
        private void addMoveDirection(float pX, float pY, float pZ, float pCalls = 1)
        {
            for (int i = 0; i < pCalls; i++)
            {
                directionCommands += pX / pCalls + " " + pY / pCalls + " " + pZ / pCalls + " ";
                calls++;
            }
        }

        
        #endregion


        #region output

        /// <summary>
        /// (Leo) Send a confirm move package based on new position
        /// </summary>
        public void SendConfMove()
        {
            //Logging.LogInfo("( " + position[0] + ", " + position[1] + ")", Logging.debugState.DETAILED);
            ConfMove _confMove = new ConfMove();
            _confMove.player = GetPlayerIndex();
            _confMove.dirX = x() + room.minX;
            _confMove.dirY = y() + room.minY;
            _confMove.dirZ = z() + room.minZ;


            _confMove.directions = directionCommands;
            directionCommands = "";
            if(currentBox != null)
            {
                currentBox.MovePosition(x(), y(), z());
                currentBox.sendBoxPackage(true);
            }

            //set y rotation
            if (orientation[0] == 0 && orientation[1] == -1) { _confMove.orientation = 180; }
            else if (orientation[0] == 1 && orientation[1] == 0) { _confMove.orientation = 90; }
            else if (orientation[0] == 0 && orientation[1] == 1) { _confMove.orientation = 0; }
            else { _confMove.orientation = -90; }


            room.sendToAll(_confMove);
        }

        /// <summary>
        /// (Ezra) Send an actuator toggle packet
        /// </summary>
        private void sendActuatorToggle(int[] pPos)
        {
            try
            {
                List<GameObject> gameObjects = room.roomArray[pPos[0], pPos[1], pPos[2]];
                foreach (GameObject gameObject in gameObjects)
                {
                    Logging.LogInfo("Player.cs: Hit an lever on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.SPAM);
                    if (gameObject is Actuator)
                    {
                    switch (gameObject.objectIndex)
                    {
                        //4 = lever
                        case (4):
                            GameObject gameObject0 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 4);
                            Lever lever = gameObject0 as Lever;
                            if (null != lever) { lever.OnInteract(GetPlayerIndex()); }
                            break;
                        //8 = button
                        case (8):
                            Logging.LogInfo("Player.cs: Hit an button on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.SPAM);
                            GameObject gameObject1 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 8);
                            Button button = gameObject1 as Button;
                            if (null != button) { button.OnInteract(GetPlayerIndex()); }
                            break;
                        //crack
                        case (12):
                            Logging.LogInfo("Player.cs: Hit a crack on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.SPAM);
                            GameObject gameObject2 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 12);
                            Crack crack = gameObject2 as Crack;
                            if (null != crack) { crack.OnInteract(GetPlayerIndex()); }
                            break;
                        default:
                            Logging.LogInfo("Player.cs: Found an actuator but couldnt handle it!", Logging.debugState.DETAILED);
                            break;
                    }


                    }
                    
                    

                    
                }
            }
            catch
            {
                //Logging.LogInfo("Player.cs: Could not get actuator!", Logging.debugState.DETAILED);
            }

        }

        /// <summary>
        /// (Leo) Send a pickup box packet
        /// </summary>
        private void sendPickUpBox(int pX, int pY, int pZ)
        {
            GameObject boxies = room.OnCoordinatesGetGameObject(pX, pY, pZ, 7);
            BoxInfo box = new BoxInfo();
            box.isPickedUp = false;
            box.ID = (boxies as Box).ID;
            box.posX = OneInFront()[0] + room.minX;
            box.posY = OneInFront()[1] + room.minY;
            box.posZ = OneInFront()[2] + room.minZ;
            room.sendToAll(box);
            currentBoxID = (boxies as Box).ID;

            //remove box at the position
            room.OnCoordinatesRemove(pX, pY, pZ, 7);
            //set player to have a box
            _hasBox = true;


        }

        /// <summary>
        /// (Leo) Send a pickup box packet
        /// </summary>
        private void sendPickUpBox(int[] pPos)
        {
            try
            {

                GameObject boxs = room.OnCoordinatesGetGameObject(pPos, 7);
                boxs.MovePosition(pPos);
                currentBox = boxs as Box;
                sendBoxPackage(currentBox, x(), y(), z() , true);

            }
            catch
            {
                Logging.LogInfo("the pPosition array length in send pickup box was not 3 long", Logging.debugState.SIMPLE);

            }
        }



        #endregion

        #region player tools

        /// <summary>
        /// (Leo) get the index the player has 
        /// </summary>
        public int GetPlayerIndex()
        {

            return playerIndex;

            Logging.LogInfo("WHHHHHHHHHHAAAAAAAAAAAAT, player is not in list you stupid fuckig dumbass", Logging.debugState.DETAILED);
            return 0;
        }

        /// <summary>
        /// (Leo) get client that is attached to the player
        /// </summary>
        public TCPMessageChannel getClient()
        {
            return _client;
        }

        /// <summary>
        /// (Leo) Returns the position the position the character is facing
        /// </summary>
        public int[] OneInFront()
        {
            int[] positionInFront = new int[3];
            positionInFront[0] = x() + orientation[0];  // xPosition + xOrientation
            positionInFront[1] = y();                   // yPosition
            positionInFront[2] = z() + orientation[1];  // zPosition + zOrientation

            return positionInFront;
        }
        #endregion

        #region update



        public override void Update()
        {
            base.Update();
            calls-= 1f/refreshSpeed;

            //walk timer
            if (walkDirection[0] != 0 || walkDirection[2] != 0)
            {
                //Logging.LogInfo("trying to walk in a direction");
                if (timer >= refreshSpeed)
                {
                    timer = 0;
                    tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                }
                timer++;

                
            }
            else
            {
                timer = 0;
            }

        }

        #endregion
    }

}
