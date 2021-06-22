using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfAirChannelToggle : ASerializable
    {
        public bool isActive;
        public int ID;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isActive);
            pPacket.Write(ID);
        }

        public override void Deserialize(Packet pPacket)
        {
            isActive = pPacket.ReadBool();
            ID = pPacket.ReadInt();
        }
    }
}
