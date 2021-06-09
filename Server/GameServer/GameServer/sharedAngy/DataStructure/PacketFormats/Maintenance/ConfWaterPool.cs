using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfWaterPool : ASerializable
    {
        public int ID;
        public int x;
        public int y;
        public int z;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(ID);
            pPacket.Write(x);
            pPacket.Write(y);
            pPacket.Write(z);
        }

        public override void Deserialize(Packet pPacket)
        {
            ID = pPacket.ReadInt();
            x = pPacket.ReadInt();
            y = pPacket.ReadInt();
            z = pPacket.ReadInt();
        }


    }
}
