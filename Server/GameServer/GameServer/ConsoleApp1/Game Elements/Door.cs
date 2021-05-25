using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Door : GameObject
    {
        GameRoom _room;
        private int ID;
        private bool isOpen = false;
        public List<Actuator> actuators = new List<Actuator>();

        public Door(GameRoom pRoom, int pX, int pY, int pID, bool pIsOpen) : base(pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            isOpen = pIsOpen;
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(6);
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
                _room.coordinatesRemove(position[0], position[1], 6);
            }
            else
            {
                //_room.coordinatesAdd(position[0], position[1], 6);
            }
            ConfDoorToggle doorToggle = new ConfDoorToggle();
            doorToggle.isActivated = isOpen;
            doorToggle.ID = ID;
            _room.sendToAll(doorToggle);
        }

        private bool checkActuators()
        {
            Logging.LogInfo("Checking actuators", Logging.debugState.DETAILED);
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
