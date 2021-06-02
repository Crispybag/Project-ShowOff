using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqMove : ASerializable
    {
        public string name;
        public int dirX;
        public int dirY;
        public int dirZ;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(name);
            pPacket.Write(dirX);
            pPacket.Write(dirY);
            pPacket.Write(dirZ);
        }

        public override void Deserialize(Packet pPacket)
        {
            name = pPacket.ReadString();
            dirX = pPacket.ReadInt();
            dirY = pPacket.ReadInt();
            dirZ = pPacket.ReadInt();
        }
    }
}
