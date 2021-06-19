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
        public Lever(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated, List<int> pRedstones) : base(pRoom, pX, pY, pZ, pID, CollInteractType.SOLID, pRedstones, pActivated)
        {
            //ID = pID;
            //room = pRoom;
            isActivated = pActivated;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 4;
        }

        public override void OnInteract(int player, ConfActuatorToggle.Object pType = ConfActuatorToggle.Object.LEVER)
        {
            base.OnInteract(player, pType);
        }
    }
}
