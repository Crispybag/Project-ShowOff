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
        public Lever(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated) : base(pRoom, pX, pY, pZ, pID, CollInteractType.SOLID,pActivated)
        {
            ID = pID;
            room = pRoom;
            isActivated = pActivated;
            room.roomArray[x(), y(), z()].Add(this);
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
                    if(room.InteractableGameobjects[door] is Door)(room.InteractableGameobjects[door] as Door).CheckDoor();
                    if(room.InteractableGameobjects[door] is AirChannel)(room.InteractableGameobjects[door] as AirChannel).CheckAirChannel();
                }
                catch
                {
                    Logging.LogInfo("Lever.cs Could not handle door, probably not in interactablegameobject list in room!", Logging.debugState.DETAILED);
                }
            }


        }
    }
}
