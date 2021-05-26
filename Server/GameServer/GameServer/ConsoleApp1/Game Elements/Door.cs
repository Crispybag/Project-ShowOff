using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Door : GameObject
    {
        private int ID;
        private bool isOpen = false;
        public List<Actuator> actuators = new List<Actuator>();

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

        public void CheckDoor()
        {
            if (checkActuators())
            {
                isOpen = true;

            }
            else
            {
                isOpen = false;

            }
            if (isOpen)
            {
                Logging.LogInfo("Door.cs: Trying to remove a object on position : " + position[0] + "," + position[1], Logging.debugState.DETAILED);
                room.OnCoordinatesRemove(position[0], position[1], position[2], 6);
            }
            else
            {
                room.OnCoordinatesAdd(position[0], position[1], position[2], 6);
            }
            ConfDoorToggle doorToggle = new ConfDoorToggle();
            doorToggle.isActivated = isOpen;
            doorToggle.ID = ID;
            room.sendToAll(doorToggle);
        }

        private bool checkActuators()
        {
            foreach (Actuator actuator in actuators)
            {
                if (!actuator.isActivated)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
