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
        public TCPMessageChannel getClient()
        {
            return _client;
        }
        public Player(GameRoom pRoom, TCPMessageChannel pClient, int pX = 0, int pY = 0) : base(pRoom, CollInteractType.SOLID)
        {
            position = new int[2] { pX, pY };
            walkDirection = new int[2];
            _room = pRoom;
            _client = pClient;
        }

        #region input
        public void addInput(ReqKeyDown.KeyType lastInput)
        {
            
            switch (lastInput)
            {
                //up
                case (ReqKeyDown.KeyType.UP):
                    changeWalkDirection(0, 1);
                    tryPositionChange(walkDirection[0], walkDirection[1]);
                    break;

                //down
                case (ReqKeyDown.KeyType.DOWN):
                    changeWalkDirection(0, -1);
                    tryPositionChange(walkDirection[0], walkDirection[1]);
                    break;
                
                //left
                case (ReqKeyDown.KeyType.LEFT):
                    changeWalkDirection(-1, 0);
                    tryPositionChange(walkDirection[0], walkDirection[1]);
                    break;
                

                //right
                case (ReqKeyDown.KeyType.RIGHT):
                    changeWalkDirection(1, 0);
                    tryPositionChange(walkDirection[0], walkDirection[1]);
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

            
            Logging.LogInfo("Player's position is now ( " + walkDirection[0] + ", " + walkDirection[1] + ")", Logging.debugState.DETAILED);

        }


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
            Logging.LogInfo("Player's position is now ( " + walkDirection[0] + ", " + walkDirection[1] + ")", Logging.debugState.DETAILED);

        }
        private void changeWalkDirection(int pX, int pY)
        {
            walkDirection = new int[2] { pX, pY };
           
        }
        private void handleInteraction()
        {
            //left
            if (position[0] > 0)
            {
                if (_room.roomArray[position[0] - 1, position[1]] == 4)
                {
                    sendActuatorToggle(position[0] - 1, position[1] );
                }
            }
            //up
            if (position[1] < 9)
            {
                if (_room.roomArray[position[0], position[1] + 1] == 4)
                {
                    sendActuatorToggle(position[0], position[1] + 1);
                }
            }
            //right
            if (position[0] < 9)
            {
                if (_room.roomArray[position[0] + 1, position[1]] == 4)
                {
                    sendActuatorToggle(position[0] + 1, position[1]);
                }
            }
            //down
            if (position[1] > 0)
            {
                if (_room.roomArray[position[0], position[1] - 1] == 4)
                {
                    sendActuatorToggle(position[0], position[1] - 1);
                }
            }
        }

        private void sendActuatorToggle(int posX, int posY)
        {
            Logging.LogInfo("Hit a lever on position : X: " + posX + " Y: " + posY, Logging.debugState.DETAILED);
            ConfActuatorToggle newToggle = new ConfActuatorToggle();
            newToggle.isActivated = true;
            newToggle.posX = posX;
            newToggle.posY = posY;
            newToggle.posZ = 0 ;
            _room.sendToAll(newToggle);
        }

        private void tryPositionChange(int pX, int pY)
        {
            int[] direction = { pX, pY };
            try
            {
                foreach(int obj in _room.roomArray[position[0] + direction[0], position[1] + direction[1]])
                    if (obj != 0)
                {
                    Logging.LogInfo("player bumped into something", Logging.debugState.DETAILED);
                }
                else
                {
                        for (int i = 0; i < _room.roomArray[position[0], position[1]].Count; i++)
                        {
                            if (_room.roomArray[position[0], position[1]][i] == 1) _room.roomArray[position[0], position[1]].Remove(i);
                        }
                    position[0] += direction[0];
                    position[1] += direction[1];
                }
                _room.roomArray[position[0], position[1]].Add(1);
                sendConfMove();
            }

            catch(Exception e)
            {
                Logging.LogInfo("probably trying to move off the grid", Logging.debugState.DETAILED);
                Logging.LogInfo(e.Message, Logging.debugState.DETAILED);
            }
        }


        private void tryCancelDirection(int pX, int pY)
        {
            if (walkDirection[0] == pX && walkDirection[1] == pY)
            {
                walkDirection[0] = 0; walkDirection[1] = 0;

            }
        }

        #endregion

        private void sendConfMove()
        {
            ConfMove _confMove = new ConfMove();
            _confMove.player = getPlayerIndex();
            _confMove.dirX = position[0];
            _confMove.dirY = position[1];
            _confMove.dirZ = 0;
            _room.sendToAll(_confMove);
        }

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



        private int timer = 0;
        public override void Update()
        {
            base.Update();


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


    }

}
