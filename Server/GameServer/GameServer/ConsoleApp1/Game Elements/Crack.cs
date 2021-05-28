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
        public Crack(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pRoom, pX, pY, pZ, pID)
        {
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            room = pRoom;
            room.roomArray[position[0], position[1], position[2]].Add(12);
            objectIndex = 12;
        }

        /// <summary>
        /// (Ezra) Once this function is called, the crack will be fixed, and will update all clients.
        /// </summary>
        public void FixCrack()
        {
            isActivated = true;
            ConfActuatorToggle crackToggle = new ConfActuatorToggle();
            crackToggle.isActived = true;
            crackToggle.obj = ConfActuatorToggle.Object.CRACK;
            crackToggle.ID = ID;
            room.sendToAll(crackToggle);
        }
    }
}
