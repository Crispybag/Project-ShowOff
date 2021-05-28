using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Actuator subclass that implements functionality for lever
    /// </summary>
    class Lever : Actuator
    {
        public Lever(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated) : base(pRoom, pX, pY, pZ, pID, pActivated)
        {
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            room = pRoom;
            isActivated = pActivated;
            room.roomArray[position[0], position[1], position[2]].Add(4);
            objectIndex = 4;
        }

        /// <summary>
        /// (Ezra) Toggles the lever, if the current state is true, it turns false, and vice versa
        /// </summary>
        public void ToggleLever()
        {
            //switch the bool and send the lever to players
            isActivated = !isActivated;
            ConfActuatorToggle newLeverToggle = new ConfActuatorToggle();
            newLeverToggle.isActived = isActivated;
            newLeverToggle.ID = ID;
            newLeverToggle.obj = ConfActuatorToggle.Object.LEVER;
            room.sendToAll(newLeverToggle);

            foreach (int door in doors)
            {
                try
                {
                    (room.InteractableGameobjects[door] as Door).CheckDoor();
                }
                catch
                {
                    Logging.LogInfo("Lever.cs Could not handle door, probably not in interactablegameobject list in room!", Logging.debugState.DETAILED);
                }
            }
        }
    }
}
