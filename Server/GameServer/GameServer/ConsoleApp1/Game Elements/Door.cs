using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Class for creating doors with door functionalities.
    /// </summary>
    public class Door : GameObject
    {
        public int ID;
        private bool isOpen = false;
        public List<int> actuators = new List<int>();

        public Door(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pIsOpen) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            isOpen = pIsOpen;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
        }

        /// <summary>
        /// (Ezra) Will check all actuator conditions connected to the door, if all return true, it will open the door.
        /// </summary>
        public void CheckDoor()
        {
            //Will return true if all actuators in the list are turned on
            if (checkActuators())
            {
                isOpen = true;

            }
            else
            {
                isOpen = false;

            }
            //if the door is open, it will remove the door, if not, it will add the door
            if (isOpen)
            {
                room.OnCoordinatesRemove(x(), y(), z(), 6);
                this.SetState(CollInteractType.PASS);
            }
            else
            {
                if (room.OnCoordinatesEmpty(x(), y(), z()))
                {
                    room.OnCoordinatesAdd(x(), y(), z(), this);
                    this.SetState(CollInteractType.SOLID);
                }
            }
            ConfDoorToggle doorToggle = new ConfDoorToggle();
            doorToggle.isActivated = isOpen;
            doorToggle.ID = ID;
            room.sendToAll(doorToggle);
        }

        private bool checkActuators()
        {
            foreach (int actuator in actuators)
            {
                try
                {
                    if (!(room.InteractableGameobjects[actuator] as Actuator).isActivated)
                    {
                        return false;
                    }
                }
                catch
                {
                    Logging.LogInfo("Door.cs: Could not handle actuator, probably not in list in room!", Logging.debugState.DETAILED);
                }
            }
            return true;
        }

    }
}
