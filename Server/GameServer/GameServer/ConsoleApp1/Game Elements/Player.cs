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
        public GameObject currentBox;
        private int[,,] position;

        private int callLoopPrevent;
        //standard stuff, along with quick coordinates
        public Player(GameRoom pRoom, TCPMessageChannel pClient, int pX = 0, int pY = 0, int pZ = 0, int pPlayerIndex = 0) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            orientation = new int[2] { 0, 1 };
            walkDirection = new int[3];
            room = pRoom;
            _client = pClient;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 1;
            playerIndex = pPlayerIndex;
        }

        #region input

        /// <summary>
        /// (Leo) adds a direction input for the server to remember that the player can move in a certain direction
        /// </summary>
        public void addInput(ReqKeyDown.KeyType lastInput)
        {

            switch (lastInput)
            {
                //up
                case (ReqKeyDown.KeyType.UP):
                    //for each of those, add a direction vector to their movement and immediately try to change position as well to minimise latency.
                    changeWalkDirection(0, 0, 1);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;

                //down
                case (ReqKeyDown.KeyType.DOWN):
                    changeWalkDirection(0, 0, -1);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;

                //left
                case (ReqKeyDown.KeyType.LEFT):
                    changeWalkDirection(-1, 0, 0);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;


                //right
                case (ReqKeyDown.KeyType.RIGHT):
                    changeWalkDirection(1, 0, 0);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1], walkDirection[2]);
                    break;

                //interaction
                case (ReqKeyDown.KeyType.INTERACTION):
                    Logging.LogInfo("Received an interaction key request", Logging.debugState.DETAILED);
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

        //change the walk direction, the direction in which the player wants to move
        private void changeWalkDirection(int pX, int pY, int pZ)
        {
            walkDirection = new int[3] { pX, pY, pZ };
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

                    //for box
                    else if (room.OnCoordinatesContain(OneInFront(), 7))
                    {
                        sendPickUpBox(OneInFront());
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
                    //5 is pressure plate
                    if (item.objectIndex != 5)
                    {
                        //if its anything else, return out of the method
                        return;
                    }
                }
                Console.WriteLine("In front is empty for box! but with interactable");
                currentBox.MovePosition(OneInFront());
                sendBoxPackage(currentBox, OneInFront(), false);
                currentBox = null;
            }


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
                boxInf.posX = position[0];
                boxInf.posY = position[1];
                boxInf.posZ = position[2];
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
                boxInf.posX = pX;
                boxInf.posY = pY;
                boxInf.posZ = pZ;
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
                Console.WriteLine(gameObject.objectIndex);
                if (gameObject.collState == CollInteractType.SOLID) return true;
                if(gameObject.objectIndex == 15)
                {
                    startDialogue(gameObject, position);
                    return false;
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

            }
        }
        //Does the check whether the player can change position or not
        private void tryPositionChange(int pX, int pY, int pZ)
        {
            //determine direction and set orientation
            int[] direction = { pX, pY, pZ };
            orientation[0] = pX;
            orientation[1] = pZ;
            Logging.LogInfo("Player's position is now ( " + x() + "," + y() + "," + z() + ")", Logging.debugState.SPAM);

            try
            {
                bool objectAtLocation = isBlockedByObject(OneInFront());

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
                sendConfMove();
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


        /// <summary>
        /// (Leo) Cancel the movement direction and then player stop :)
        /// </summary>
        private void tryCancelDirection(int pX, int pY, int pZ)
        {
            if (walkDirection[0] == pX && walkDirection[1] == pY && walkDirection[2] == pZ)
            {
                walkDirection[0] = 0; walkDirection[1] = 0; walkDirection[2] = 0;

            }
        }
        #endregion


        #region output

        /// <summary>
        /// (Leo) Send a confirm move package based on new position
        /// </summary>
        private void sendConfMove()
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
                sendBoxPackage(currentBox, x(), y(), z(), true);
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
                List<GameObject> actuators = room.roomArray[pPos[0], pPos[1], pPos[2]];
                foreach (GameObject actuator in actuators)
                {
                    switch (actuator.objectIndex)
                    {
                        //4 = lever
                        case (4):
                            Logging.LogInfo("Player.cs: Hit an lever on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.DETAILED);
                            GameObject gameObject = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 4);
                            Lever lever = gameObject as Lever;
                            if (null != lever) { lever.ToggleLever(); }
                            break;
                        //8 = button
                        case (8):
                            Logging.LogInfo("Player.cs: Hit an button on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.DETAILED);
                            GameObject gameObject1 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 8);
                            Button button = gameObject1 as Button;
                            if (null != button) { button.SetActive(); }
                            break;
                        //crack
                        case (12):
                            Logging.LogInfo("Player.cs: Hit a crack on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.DETAILED);
                            GameObject gameObject2 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 12);
                            Crack crack = gameObject2 as Crack;
                            if (null != crack) { crack.FixCrack(); }
                            break;

                        default:
                            Logging.LogInfo("Player.cs: Found an actuator but couldnt handle it!", Logging.debugState.DETAILED);
                            break;
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


            //send package
/*            ConfHandleBox confHandleBox = new ConfHandleBox();
            confHandleBox.posX = pX;
            confHandleBox.posY = pY;
            confHandleBox.posZ = pZ;
            confHandleBox.isPickingUp = true;*/
        }

        /// <summary>
        /// (Leo) Send a pickup box packet
        /// </summary>
        private void sendPickUpBox(int[] pPos)
        {
            try
            {
/*                GameObject boxies = room.OnCoordinatesGetGameObject(pPos, 7);

                BoxInfo box = new BoxInfo();
                box.isPickedUp = false;
                box.ID = (boxies as Box).ID;
                box.posX = OneInFront()[0];
                box.posY = OneInFront()[1];
                box.posZ = OneInFront()[2];
                room.sendToAll(box);

                currentBoxID = (boxies as Box).ID;

                //remove box at the position
                room.OnCoordinatesRemove(pPos[0], pPos[1], pPos[2], 7);
                //set player to have a box
                _hasBox = true;*/

                GameObject boxs = room.OnCoordinatesGetGameObject(pPos, 7);
                currentBox = boxs;
                boxs.MovePosition(pPos);
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
                if (timer >= 1)
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
