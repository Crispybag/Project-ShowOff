using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Class for creating doors with door functionalities.
    /// </summary>
    public class Door : RedstoneOutput
    {
        //public int ID;
        //private bool isOpen = false;
        //public List<int> actuators = new List<int>();

        public Door(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pIsOpen, List<int> pActuators) : base(pRoom, pX, pY, pZ, pID, CollInteractType.SOLID, pActuators, pIsOpen)
        {
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 6;
        }


        public override void CheckOutput()
        {
            base.CheckOutput();

            //if the door is open, it will remove the door, if not, it will add the door
            if (isActivated)
            {
                room.OnCoordinatesRemove(x(), y(), z(), 6);
                this.SetState(CollInteractType.PASS);
            }
            else
            {
                this.SetState(CollInteractType.SOLID);
                room.OnCoordinatesAdd(x(), y(), z(), this);
            }
            ConfDoorToggle doorToggle = new ConfDoorToggle();
            doorToggle.isActivated = isActivated;
            doorToggle.ID = ID;
            room.sendToAll(doorToggle);
        }

    }
}
