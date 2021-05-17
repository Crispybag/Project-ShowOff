using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqMove : ASerializable
    {
        public int oldX;
        public int oldY;
        public int oldZ;
        public bool bubby;
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
