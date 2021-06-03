using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqResetLevel : ASerializable
    {
        public bool wantsReset = true;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(wantsReset);
        }

        public override void Deserialize(Packet pPacket)
        {
            wantsReset = pPacket.ReadBool();
        }
    }
}
