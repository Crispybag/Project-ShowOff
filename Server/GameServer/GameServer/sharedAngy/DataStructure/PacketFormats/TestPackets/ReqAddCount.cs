using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqAddCount : ASerializable
    {
        public bool isValid;
        public override void Deserialize(Packet pPacket)
        {
            isValid = pPacket.ReadBool();
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isValid);
        }
    }
}
