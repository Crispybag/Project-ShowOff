﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfActuatorToggle : ASerializable
    {
        public ActuatorState currentState;
        public int posX;
        public int posY;
        public int posZ;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)currentState);
            pPacket.Write(posX);
            pPacket.Write(posY);
            pPacket.Write(posZ);
        }

        public override void Deserialize(Packet pPacket)
        {
            currentState = (ActuatorState)pPacket.ReadInt();
            posX = pPacket.ReadInt();
            posY = pPacket.ReadInt();
            posZ = pPacket.ReadInt();
        }

        public enum ActuatorState
        {
            TRUE,
            FALSE,
            TOGGLE
        }
    }
}
