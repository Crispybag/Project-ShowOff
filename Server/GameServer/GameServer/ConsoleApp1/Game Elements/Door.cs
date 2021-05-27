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
