using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqPlayerSwitch : ASerializable
    {
        public int i = 0;

        public override void Deserialize(Packet pPacket)
        {
            i = pPacket.ReadInt();
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(i);
        }
    }
}
