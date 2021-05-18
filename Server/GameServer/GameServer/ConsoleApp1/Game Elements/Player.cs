using System;
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
        public Player(GameRoom pRoom, TCPMessageChannel pClient) : base(CollInteractType.SOLID)
        {
            position = new int[2];
            _room = pRoom;
            _client = pClient;
        }

        public void checkInput(ReqKeyDown.KeyType lastInput)
        {
            
            switch (lastInput)
            {
                //up
                case (ReqKeyDown.KeyType.UP):
                    if (position[1] == 0)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }
                    else if (_room.roomArray[position[0],position[1] - 1] != 0)
                    {
                        Logging.LogInfo("played bumped into something!", Logging.debugState.SPAM);
                        
                        
                    }
                    else
                    {
                        //move player 1 up
                        _room.roomArray[position[0], position[1]] = 0;
                        position[1]--;
                    }
                    break;

                //down
                case (ReqKeyDown.KeyType.DOWN):
                    if (position[1] == _room.roomArray.GetLength(1) - 1)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }
                    else if (_room.roomArray[position[0], position[1] + 1] != 0)
                    {
                        Logging.LogInfo("played bumped into something!", Logging.debugState.SPAM);


                    }

                    else
                    {
                        //move player 1 Left
                        _room.roomArray[position[0], position[1]] = 0;
                        position[1]++;
                    }
                    break;
                
                //left
                case (ReqKeyDown.KeyType.LEFT):
                    if (position[0] == 0)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }

                    else if (_room.roomArray[position[0]-1, position[1]] != 0)
                    {
                        Logging.LogInfo("played bumped into something!", Logging.debugState.SPAM);


                    }

                    else
                    {
                        //move player 1 Left
                        _room.roomArray[position[0], position[1]] = 0;
                        position[0]--;
                    }
                    break;
                

                //right
                case (ReqKeyDown.KeyType.RIGHT):
                    if (position[0] == _room.roomArray.GetLength(0) - 1)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }
                    else if (_room.roomArray[position[0]+1, position[1]] != 0)
                    {
                        Logging.LogInfo("played bumped into something!", Logging.debugState.SPAM);


                    }
                    else
                    {
                        //move player 1 Left
                        _room.roomArray[position[0], position[1]] = 0;
                        position[0]++;
                    }
                    break;

            }

            _room.roomArray[position[0], position[1]] = 1;
            Logging.LogInfo("Player's position is now ( " + position[0] + ", " + position[1] + ")", Logging.debugState.DETAILED);
        }
    }
}
