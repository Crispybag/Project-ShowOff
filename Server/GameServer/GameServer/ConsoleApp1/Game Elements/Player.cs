using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    class Player : GameObject
    {
        private GameRoom _room;
        public Player(GameRoom pRoom) : base(CollInteractType.SOLID)
        {
            position = new int[2];
            _room = pRoom;
        }

        public void checkInput(int lastInput)
        {
            
            switch (lastInput)
            {
                //up
                case (0):
                    if (position[1] == 0)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }
                    else
                    {
                        //move player 1 up
                        position[1]--;
                    }
                    break;

                //down
                case (1):
                    if (position[1] == _room.roomArray.GetLength(1) - 1)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }
                    else
                    {
                        //move player 1 Left
                        position[1]++;
                    }
                    break;
                
                //left
                case (2):
                    if (position[0] == 0)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }
                    else
                    {
                        //move player 1 Left
                        position[0]--;
                    }
                    break;
                

                //right
                case (3):
                    if (position[0] == _room.roomArray.GetLength(0) - 1)
                    {
                        Logging.LogInfo("movement ignored, player at edge", Logging.debugState.SPAM);
                    }
                    else
                    {
                        //move player 1 Left
                        position[0]++;
                    }
                    break;

            }
        }
    }
}
