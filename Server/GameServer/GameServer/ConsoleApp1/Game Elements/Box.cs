using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    public class Box : GameObject
    {
        GameRoom _room;
        public Box(GameRoom pRoom, int pX, int pY) : base(pRoom, CollInteractType.SHOVE)
        {
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(2);
        }

        public bool CanBeShoved(int pPosX, int pPosY)
        {
            //try check for edge of array
            try
            {
                //check if coordinates you are trying to shove to is empty
                if (_room.coordinatesEmpty(pPosX, pPosY))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void TryShove(int pDirX, int pDirY)
        {
            if (CanBeShoved(position[0] + pDirX, position[1] + pDirY))
            {

            }
        }


    }
}
