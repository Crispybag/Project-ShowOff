using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfTele : ASerializable
    {
        public int dirX;
        public int dirY;
        public int dirZ;
        public bool oh;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(dirX);
            pPacket.Write(dirY);
            pPacket.Write(dirZ);
        }

        public override void Deserialize(Packet pPacket)
        {
            dirX = pPacket.ReadInt();
            dirY = pPacket.ReadInt();
            dirZ = pPacket.ReadInt();
        }
    }
}
