using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfHandleBox : ASerializable
    {
        public bool isPickingUp = true;
        public int posX = 0;
        public int posY = 0;
        public int posZ = 0;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isPickingUp);
            pPacket.Write(posX);
            pPacket.Write(posY);
            pPacket.Write(posZ);
        }

        public override void Deserialize(Packet pPacket)
        {
            pPacket.Write(isPickingUp);
            pPacket.Write(posX);
            pPacket.Write(posY);
            pPacket.Write(posZ);
        }

    }
}
