using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using sharedAngy;

namespace Server
{
    public class Player : GameObject
    {
        private GameRoom _room;
        private TCPMessageChannel _client;
        private int[] walkDirection;

        private int timer = 0;

        //standard stuff, along with quick coordinates
        public Player(GameRoom pRoom, TCPMessageChannel pClient, int pX = 0, int pY = 0) : base(pRoom, CollInteractType.SOLID)
        {
            position = new int[2] { pX, pY };
            walkDirection = new int[2];
            _room = pRoom;
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
                    changeWalkDirection(0, 1);
                    if (timer == 0)tryPositionChange(walkDirection[0], walkDirection[1]);
                    break;

                //down
                case (ReqKeyDown.KeyType.DOWN):
                    changeWalkDirection(0, -1);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1]);
                    break;
                
                //left
                case (ReqKeyDown.KeyType.LEFT):
                    changeWalkDirection(-1, 0);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1]);
                    break;
                

                //right
                case (ReqKeyDown.KeyType.RIGHT):
                    changeWalkDirection(1, 0);
                    if (timer == 0) tryPositionChange(walkDirection[0], walkDirection[1]);
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
                    tryCancelDirection(0, 1);
                    break;

                case (ReqKeyUp.KeyType.DOWN):
                    tryCancelDirection(0, -1);
                    break;

                case (ReqKeyUp.KeyType.LEFT):
                    tryCancelDirection(-1, 0);
                    break;

                case (ReqKeyUp.KeyType.RIGHT):
                    tryCancelDirection(1, 0);
                    break;




            }
            //Logging.LogInfo("Player's position is now ( " + walkDirection[0] + ", " + walkDirection[1] + ")", Logging.debugState.DETAILED);

        }

        //change the walk direction, the direction in which the player wants to move
        private void changeWalkDirection(int pX, int pY)
        {
            walkDirection = new int[2] { pX, pY };
           
        }

        //Handles the interaction button, check if there are any interactionables around, and interacts with them
        private void handleInteraction()
        {
            //from player position it will check in a plus sign if there is an actuator
            try
            {
                //left
                if (position[0] > 0)
                {
                    if (_room.coordinatesContain(position[0] - 1, position[1], 4))
                    {
                        sendActuatorToggle(position[0] - 1, position[1]);
                    }
                }
                //up
                if (position[1] < 9)
                {
                    if (_room.coordinatesContain(position[0], position[1] + 1, 4))
                    {
                        sendActuatorToggle(position[0], position[1] + 1);
                    }
                }

                //right
                if (position[0] < 9)
                {
                    if (_room.coordinatesContain(position[0] + 1, position[1], 4))
                    {
                        sendActuatorToggle(position[0] + 1, position[1]);
                    }
                }

                //down
                if (position[1] > 0)
                {
                    if (_room.coordinatesContain(position[0], position[1] - 1, 4))
                    {
                        sendActuatorToggle(position[0], position[1] - 1);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.LogInfo("Player.cs: " + e.Message, Logging.debugState.DETAILED);
            }



        }

        
        //Does the check whether the player can change position or not
        private void tryPositionChange(int pX, int pY)
        {
            int[] direction = { pX, pY };
            try
            {
                bool objectAtLocation = false;
                //check if the list contains something that is not 0
                foreach (int obj in _room.roomArray[position[0] + direction[0], position[1] + direction[1]])
                {
                    if (obj != 0)
                    {
                        switch(obj)
                        {
                            //box interaction
                            case (7):

                                Box pBox = null;
                                
                                // try and get the game object on the position if it is a box
                                GameObject gameObject = _room.coordinatesGetGameObject(position[0] + pX, position[1] + pY, 7);
                                
                                //set box value to that box
                                if (gameObject is Box) pBox = gameObject as Box;

                                //a quick safeguard check if the box shoving works
                                if (pBox != null)
                                { 

                                    //if it can be shoved 2 tiles in that direction
                                    if (pBox.CanBeShoved(position[0] + 2 * pX, position[1] + 2* pY))
                                    {
                                        //try shoving the box (it should always be able to)
                                        pBox.TryShove(pX, pY);

                                        //move player
                                        _room.coordinatesRemove(position[0], position[1], 1);
                                        position[0] += direction[0];
                                        position[1] += direction[1];
                                        //add
                                        _room.roomArray[position[0], position[1]].Add(1);
                                        sendConfMove();
                                    }
                                }

                                break;

                            //presure plate
                            case (5):
                                objectAtLocation = false;
                                break;
                            default:
                                //player bumps into something code
                                Logging.LogInfo("player bumped into something", Logging.debugState.DETAILED);
                                objectAtLocation = true;
                                break;
                        }

                    }
                }

                //Passes the check and can move
                if (!objectAtLocation)
                {
                //change the location of the player and remove the player value from the grid at the place the player was
                _room.coordinatesRemove(position[0], position[1], 1);
                    position[0] += direction[0];
                    position[1] += direction[1];
                    //add
                    _room.roomArray[position[0], position[1]].Add(1);
                    sendConfMove();
                //Logging.LogInfo("Player's position is now ( " + position[0] + ", " + position[1] + ")", Logging.debugState.DETAILED);

                }


            }

            catch(Exception e)
            {
                Logging.LogInfo("probably trying to move off the grid", Logging.debugState.DETAILED);
                Logging.LogInfo(e.Message, Logging.debugState.DETAILED);
            }
        }

        //cancel the movement direction and then player stop :)
        private void tryCancelDirection(int pX, int pY)
        {
            if (walkDirection[0] == pX && walkDirection[1] == pY)
            {
                walkDirection[0] = 0; walkDirection[1] = 0;

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
            _confMove.dirZ = 0;
            _room.sendToAll(_confMove);
            _room.printGrid(_room.roomArray);
        }

        //Send an actuator toggle packet
        private void sendActuatorToggle(int posX, int posY)
        {
            Logging.LogInfo("Hit a actuator on position : X: " + posX + " Y: " + posY, Logging.debugState.DETAILED);

            //4 is actuator
            try
            {
                GameObject obj = _room.coordinatesGetGameObject(posX, posY, 4);
                Actuator actuator = obj as Actuator;
                actuator.isActivated = !actuator.isActivated;

                foreach(Door door in actuator.doors)
                {
                    door.CheckDoor();
                }

                ConfActuatorToggle newToggle = new ConfActuatorToggle();
                newToggle.isActived = actuator.isActivated;
                newToggle.ID = actuator.ID;
                newToggle.obj = ConfActuatorToggle.Object.LEVER;
                _room.sendToAll(newToggle);
            }
            catch
            {
                Logging.LogInfo("Player.cs: Could not get actuator!", Logging.debugState.DETAILED);
            }
        }
        #endregion

        #region player tools
        
        //get the index the player has 
        private int getPlayerIndex()
        {
            for (int i = 0; i < _room.players.Count; i++)
            {
                if (this == _room.players[i])
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
                    tryPositionChange(walkDirection[0], walkDirection[1]);
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
