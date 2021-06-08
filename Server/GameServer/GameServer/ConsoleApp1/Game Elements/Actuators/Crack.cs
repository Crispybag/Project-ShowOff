using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Sub class of actuator, because it has very similar functionalities to actuator, after activated once, it has been fixed.
    /// </summary>
    class Crack : Actuator
    {
        private bool canActivate = true;
        public Crack(GameRoom pRoom, int pX, int pY, int pZ, int pID, List<int> pRedstones) : base(pRoom, pX, pY, pZ, pID, CollInteractType.SOLID, pRedstones)
        {
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 12;
        }

        public override void OnInteract(ConfActuatorToggle.Object pType = ConfActuatorToggle.Object.CRACK)
        {
            //make sure it can only be activated once
            if (canActivate)
            {
                base.OnInteract(pType);
            }

            canActivate = false;
            room.OnCoordinatesRemove(x(), y(), z(), 12);
        }

    }
}
