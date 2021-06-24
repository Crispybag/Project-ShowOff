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

        private int callLoopPrevent;
        public bool wantsReset;
        private int cameraRotation;
        //standard stuff, along with quick coordinates
        public enum PlayerType
        {
            ALEX = 0,
            NUC = 1
        }

        public PlayerType playerType;


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

            playerType = pPlayerType;
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
            //Logging.LogInfo("Player's position is now ( " + walkDirection[0] + ", " + walkDirection[1] + ")", Logging.debugState.DETAILED);

        }

        public void SetResetStatus(bool pReset)
        {
            wantsReset = !wantsReset;
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
                        sendActuatorToggle(OneInFront());
                    }

                    //for button
                    if (room.OnCoordinatesContain(OneInFront(), 8))
                    {
                        sendActuatorToggle(OneInFront());
                    }

                    //for crack
                    if (room.OnCoordinatesContain(OneInFront(), 12))
                    {
                        sendActuatorToggle(OneInFront());
                    }

                    //for box
                    else if (room.OnCoordinatesContain(OneInFront(), 7))
                    {
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
                    currentBox.SendBoxPackage(currentBox, new int[3] { currentBox.x(), currentBox.y(), currentBox.z() }, false);

                    handleWaterInteractionBox();

                    sendBoxPackage(currentBox, OneInFront(), false);
                    currentBox = null;
                }
                catch { Logging.LogInfo("One In Front does not return an array that is 3 in length", Logging.debugState.SIMPLE); }

            }
            //if its not empty, there are a few exceptions like the pressure plate
            else
            {
                Console.WriteLine("Box is checking in front!");
                List<GameObject> index = room.OnCoordinatesGetIndexes(OneInFront());
                foreach (GameObject item in index)
                {
                    //5 is pressure plate, 16 is water
                    if (item.objectIndex != 5 && item.objectIndex != 16)
                    {
                        //if its anything else, return out of the method
                        return;
                    }
                }
                Console.WriteLine("In front is empty for box! but with interactable");
                currentBox.MovePosition(OneInFront());
                currentBox.SendBoxPackage(currentBox, OneInFront(), false);

                handleWaterInteractionBox();

                sendBoxPackage(currentBox, OneInFront(), false);
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

        private void sendBoxPackage(GameObject box, int[] position, bool pIsPickedUp)
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

                //check if the player can move on the slope and move on it when the player can move on the slope
                if (pSlope.CanMoveOnSlope(pPosition, orientation))
                {
                    MovePosition(pSlope.MoveOnSlope(pPosition));
                }
                else
                {
                    checkSpecialCollision(pSlope.MoveOnSlope(pPosition));

                }
            }

            //else it could be the s2 position so check that as well here
            else if (room.OnCoordinatesGetGameObject(pPosition[0] + orientation[0], pPosition[1] - 1, pPosition[2] + orientation[1], 10) is Slope)
            {
                //if that is true return the slope on that coordinate to the player
                Slope pSlope = room.OnCoordinatesGetGameObject(pPosition[0] + orientation[0], pPosition[1] - 1, pPosition[2] + orientation[1], 10) as Slope;

                //check with that slope whether the player can move on it
                if (pSlope.CanMoveOnSlope(pPosition, orientation))
                {
                    MovePosition(pSlope.MoveOnSlope(pPosition));
                }
                else
                {
                    checkSpecialCollision(pSlope.MoveOnSlope(pPosition));
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
                    AirChannel airChannel = room.OnCoordinatesGetGameObject(pPosition, 13) as AirChannel;
                    if (airChannel.CanPushPlayer(pPosition))
                    {
                        MovePosition(airChannel.PushPlayer(pPosition));
                    }
                    else
                    {
                        checkSpecialCollision(airChannel.PushPlayer(pPosition));
                    }
                }
                else
                {
                    MoveDirection(orientation[0], 0, orientation[1]);
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
            if (callLoopPrevent >= 20)
            {
                Logging.LogInfo("potential infinite recursive loop in check special collision, make sure that the code runs properly or increase treshold", Logging.debugState.DETAILED);
                return;
            }

            foreach (GameObject index in objectsOnCoord)
            {
                switch (index.objectIndex)
                {
                    //slope hit at s0
                    case (10):
                        handleS0Hit(pPosition);
                        callLoopPrevent = 0;
                        break;

                        //airchannel
                    case (13):
                        handleAirChannelHit(pPosition);
                        callLoopPrevent = 0;
                        break;

                    default:
                        break;
                }
            }
        }
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
                        MoveDirection(0, -1, 0);
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
                    MoveDirection(direction);
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

        /// <summary>
        /// (Ezra) Starts new dialogue
        /// </summary>
        private void startDialogue(GameObject diaobj, int[] direction)
        {
            Dialogue dia = diaobj as Dialogue;
            ConfProgressDialogue progressDialogue = new ConfProgressDialogue();
            progressDialogue.ID = dia.ID;
            room.currentDialogue = dia.ID;
            room.sendToAll(progressDialogue);
            room.OnCoordinatesRemove(OneInFront(), diaobj.objectIndex);
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

            if(currentBox != null)
            {
                currentBox.MovePosition(x(), y(), z());
                currentBox.SendBoxPackage(currentBox, x(), y(), z(), true);
            }

            //set y rotation
            if (orientation[0] == 0 && orientation[1] == -1) { _confMove.orientation = 0; }
            else if (orientation[0] == 1 && orientation[1] == 0) { _confMove.orientation = 90; }
            else if (orientation[0] == 0 && orientation[1] == 1) { _confMove.orientation = 180; }
            else { _confMove.orientation = -90; }


            room.sendToAll(_confMove);
            //room.PrintGrid(room.roomArray);
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
                            if (null != lever) { lever.OnInteract(); }
                            break;
                        //8 = button
                        case (8):
                            Logging.LogInfo("Player.cs: Hit an button on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.SPAM);
                            GameObject gameObject1 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 8);
                            Button button = gameObject1 as Button;
                            if (null != button) { button.OnInteract(); }
                            break;
                        //crack
                        case (12):
                            Logging.LogInfo("Player.cs: Hit a crack on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.SPAM);
                            GameObject gameObject2 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 12);
                            Crack crack = gameObject2 as Crack;
                            if (null != crack) { crack.OnInteract(); }
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

            //walk timer
            if (walkDirection[0] != 0 || walkDirection[2] != 0)
            {
                //Logging.LogInfo("trying to walk in a direction");
                if (timer >= 2)
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
