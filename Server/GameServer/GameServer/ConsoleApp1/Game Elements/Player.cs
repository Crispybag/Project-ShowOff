﻿using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    class Player : GameObject
    {
        private GameRoom _room;
        private TCPMessageChannel _client;

        public TCPMessageChannel getClient()
        {
            return _client;
        }
        public Player(GameRoom pRoom, TCPMessageChannel pClient, int pX = 0, int pY = 0) : base(CollInteractType.SOLID)
        {
            position = new int[2] { pX, pY };
            _room = pRoom;
            _client = pClient;
        }

        #region input
        public void checkInput(ReqKeyDown.KeyType lastInput)
        {
            
            switch (lastInput)
            {
                //up
                case (ReqKeyDown.KeyType.UP):
                    tryPositionChange(0, 1);
                    break;

                //down
                case (ReqKeyDown.KeyType.DOWN):
                    tryPositionChange(0, -1);
                    break;
                
                //left
                case (ReqKeyDown.KeyType.LEFT):
                    tryPositionChange(-1, 0);
                    break;
                

                //right
                case (ReqKeyDown.KeyType.RIGHT):
                    tryPositionChange(1, 0);
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

            _room.roomArray[position[0], position[1]] = 1;
            sendConfMove();
            Logging.LogInfo("Player's position is now ( " + position[0] + ", " + position[1] + ")", Logging.debugState.DETAILED);
        }

        private void handleInteraction()
        {
            int[] left = new int[position[0] - 1, position[1]];
            int[] up = new int[position[0], position[1] + 1];
            int[] right = new int[position[0] + 1, position[1]];
            int[] down = new int[position[0], position[1] - 1];

            Logging.LogInfo("Checking the following positions for a lever: left: " + left + " up: " + up + " right: " + right + " down: " + down , Logging.debugState.DETAILED);

            if (_room.roomArray[left] == 4)
            {
                Logging.LogInfo("Hit a lever!", Logging.debugState.DETAILED);
            }
            if (_room.roomArray[up] == 4)
            {
                Logging.LogInfo("Hit a lever!", Logging.debugState.DETAILED);
            }
            if (_room.roomArray[right] == 4)
            {
                Logging.LogInfo("Hit a lever!", Logging.debugState.DETAILED);
            }
            if (_room.roomArray[down] == 4)
            {
                Logging.LogInfo("Hit a lever!", Logging.debugState.DETAILED);
            }
        }


        private void tryPositionChange(int pX, int pY)
        {
            int[] direction = { pX, pY };
            try
            {
                if (_room.roomArray[position[0] + direction[0], position[1] + direction[1]] != 0)
                {
                    Logging.LogInfo("player bumped into something", Logging.debugState.DETAILED);
                }
                else
                {
                    _room.roomArray[position[0], position[1]] = 0;
                    position[0] += direction[0];
                    position[1] += direction[1];
                }
            }

            catch(Exception e)
            {
                Logging.LogInfo("probably trying to move off the grid", Logging.debugState.DETAILED);
                Logging.LogInfo(e.Message, Logging.debugState.DETAILED);
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

    }

}