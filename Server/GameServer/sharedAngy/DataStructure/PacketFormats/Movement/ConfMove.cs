using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfMove : ASerializable
    {
        public int oldX;
        public int oldY;
        public int oldZ;
        public bool oh;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(oldX);
            pPacket.Write(oldY);
            pPacket.Write(oldZ);
        }

        public override void Deserialize(Packet pPacket)
        {
            oldX = pPacket.ReadInt();
            oldY = pPacket.ReadInt();
            oldZ = pPacket.ReadInt();
        }
    }
}
