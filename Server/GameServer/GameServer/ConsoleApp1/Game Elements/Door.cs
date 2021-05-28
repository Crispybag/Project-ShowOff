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

        public Door(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pIsOpen) : base(pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            isOpen = pIsOpen;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            room = pRoom;
            room.roomArray[position[0], position[1], position[2]].Add(6);
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
                room.OnCoordinatesRemove(position[0], position[1], position[2], 6);
            }
            else
            {
                if (room.OnCoordinatesEmpty(position))
                {
                    room.OnCoordinatesAdd(position[0], position[1], position[2], 6);
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
