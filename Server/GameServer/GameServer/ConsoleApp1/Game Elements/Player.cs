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


        //standard stuff, along with quick coordinates
        public Player(GameRoom pRoom, TCPMessageChannel pClient, int pX = 0, int pY = 0, int pZ = 0) : base(pRoom, CollInteractType.SOLID)
        {
            position = new int[3] { pX, pY, pX };
            orientation = new int[2] { 0, 1 };
            walkDirection = new int[3];
            room = pRoom;
            _client = pClient;
            objectIndex = 1;
        }

        #region input

        //adds a direction input for the server to remember that the player can move in a certain direction
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

        //handle the release of the input where you remove the input from the list and cancel the direction if it is the direction the player is currently moving in
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

        //Handles the interaction button, check if there are any interactionables around, and interacts with them
        private void handleInteraction()
        {

            try
            {

                if (!_hasBox)
                {
                    //for lever
                    if (room.OnCoordinatesContain(OneInFront(), 4))
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
                    //check if box can be placed (at position + orientation)
                    if (room.OnCoordinatesEmpty(OneInFront()) )
                    {
                        //place box
                        try { room.roomArray[OneInFront()[0], OneInFront()[1], OneInFront()[2]].Add(7); }
                        catch { Logging.LogInfo("One In Front does not return an array that is 3 in length", Logging.debugState.SIMPLE); }
                        _hasBox = false;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.LogInfo("Player.cs: " + e.Message, Logging.debugState.DETAILED);
            }
        }


        //Does the check whether the player can change position or not
        private void tryPositionChange(int pX, int pY, int pZ)
        {
            int[] direction = { pX, pY, pZ };
            orientation[0] = pX;
            orientation[1] = pZ;
            Logging.LogInfo("Player's position is now ( " + position[0] + "," + position[1] + "," + position[2] + ")", Logging.debugState.DETAILED);
            try
            {
                bool objectAtLocation = false;
                //check if the list contains something that is not 0
                foreach (int obj in room.roomArray[position[0] + direction[0], position[1] + direction[1], position[2] + direction[2]])
                {
                    if (obj != 0)
                    {
                        objectAtLocation = true;
                        break;
                    }
                }

                //Passes the check and can move
                if (!objectAtLocation)
                {
                    //change the location of the player and remove the player value from the grid at the place the player was
                    room.OnCoordinatesRemove(position[0], position[1], position[2], 1);
                    position[0] += direction[0];
                    position[1] += direction[1];
                    position[2] += direction[2];
                    //add
                    room.roomArray[position[0], position[1], position[2]].Add(1);
                    Logging.LogInfo("Player's position is now ( " + position[0] + "," + position[1] + "," + position[2] + ")", Logging.debugState.DETAILED);
                    sendConfMove();
                }


            }

            catch (Exception e)
            {
                Logging.LogInfo("probably trying to move off the grid", Logging.debugState.DETAILED);
                Logging.LogInfo(e.Message, Logging.debugState.DETAILED);
            }
        }

        //cancel the movement direction and then player stop :)
        private void tryCancelDirection(int pX, int pY, int pZ)
        {
            if (walkDirection[0] == pX && walkDirection[1] == pY && walkDirection[2] == pZ)
            {
                walkDirection[0] = 0; walkDirection[1] = 0; walkDirection[2] = 0;

            }
        }


        #endregion

        #region output

        //Send a confirm move package based on new position
        private void sendConfMove()
        {
            //Logging.LogInfo("( " + position[0] + ", " + position[1] + ")", Logging.debugState.DETAILED);
            ConfMove _confMove = new ConfMove();
            _confMove.player = getPlayerIndex();
            _confMove.dirX = position[0];
            _confMove.dirY = position[1];
            _confMove.dirZ = position[2];

            //set y rotation
            if (orientation[0] == 0 && orientation[1] == -1) { _confMove.orientation = 0; }
            else if (orientation[0] == 1 && orientation[1] == 0) { _confMove.orientation = 90; }
            else if (orientation[0] == 0 && orientation[1] == 1) { _confMove.orientation = 180; }
            else { _confMove.orientation = -90; }


            room.sendToAll(_confMove);
            room.PrintGrid(room.roomArray);
        }

        //Send an actuator toggle packet
        private void sendActuatorToggle(int[] pPos)
        {
            try
            {
                List<int> actuators = room.roomArray[pPos[0], pPos[1], pPos[2]];
                foreach (int actuator in actuators)
                {
                    switch (actuator)
                    {
                        //4 = lever
                        case (4):
                            Logging.LogInfo("Player.cs: Hit an lever on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.DETAILED);
                            GameObject gameObject = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 4);
                            Actuator lever = gameObject as Actuator;
                            lever.isActivated = !lever.isActivated;

                            foreach (Door door in lever.doors)
                            {
                                door.CheckDoor();
                            }

                            ConfActuatorToggle newLeverToggle = new ConfActuatorToggle();
                            newLeverToggle.isActived = lever.isActivated;
                            newLeverToggle.ID = lever.ID;
                            newLeverToggle.obj = ConfActuatorToggle.Object.LEVER;
                            room.sendToAll(newLeverToggle);
                            break;
                        //8 = button
                        case (8):
                            Logging.LogInfo("Player.cs: Hit an button on position : " + pPos[0] + "," + pPos[1] + "," + pPos[2] + "!", Logging.debugState.DETAILED);
                            GameObject gameObject1 = room.OnCoordinatesGetGameObject(pPos[0], pPos[1], pPos[2], 8);
                            Button button = gameObject1 as Button;
                            if (null != button)
                            {
                                button.SetActive();
                                if (button.elevators.Count > 0)
                                {
                                    foreach (Elevator elevator in button.elevators)
                                    {
                                        elevator.NextPosition(button.currentDirection);
                                    }
                                }

                            }
                            break;
                    }



                }
            }
            catch
            {
                //Logging.LogInfo("Player.cs: Could not get actuator!", Logging.debugState.DETAILED);
            }

        }

        //Send a pickup box packet
        private void sendPickUpBox(int pX, int pY, int pZ)
        {
            //remove box at the position
            room.OnCoordinatesRemove(pX, pY, pZ, 7);
            //set player to have a box
            _hasBox = true;

            //send package
            ConfHandleBox confHandleBox = new ConfHandleBox();
            confHandleBox.posX = pX;
            confHandleBox.posY = pY;
            confHandleBox.posZ = pZ;
            confHandleBox.isPickingUp = true;
        }
        private void sendPickUpBox(int[] pPos)
        {
            try
            {
                //remove box at the position
                room.OnCoordinatesRemove(pPos[0], pPos[1], pPos[2], 7);
                //set player to have a box
                _hasBox = true;

                //send package
                ConfHandleBox confHandleBox = new ConfHandleBox();
                confHandleBox.posX = pPos[0];
                confHandleBox.posY = pPos[1];
                confHandleBox.posZ = pPos[2];
                confHandleBox.isPickingUp = true;
            }
            catch
            {
                Logging.LogInfo("the pPosition array length in send pickup box was not 3 long", Logging.debugState.SIMPLE);

            }
        }



        #endregion

        #region player tools

        //get the index the player has 
        private int getPlayerIndex()
        {
            for (int i = 0; i < room.players.Count; i++)
            {
                if (this == room.players[i])
                {
                    return i;
                }
            }
            Logging.LogInfo("WHHHHHHHHHHAAAAAAAAAAAAT, player is not in list you stupid fuckig dumbass", Logging.debugState.DETAILED);
            return 0;
        }

        //get client that is attached to the player
        public TCPMessageChannel getClient()
        {
            return _client;
        }

        public int[] OneInFront()
        {
            int[] positionInFront = new int[3];
            positionInFront[0] = position[0] + orientation[0];  // xPosition + xOrientation
            positionInFront[1] = position[1];                   // yPosition
            positionInFront[2] = position[2] + orientation[1];  // zPosition + zOrientation

            return positionInFront;
        }
        #endregion

        #region update



        public override void Update()
        {
            base.Update();

            //walk timer
            if (walkDirection[0] != 0 || walkDirection[1] != 0)
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
