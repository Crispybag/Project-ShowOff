using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqActuatorToggle : ASerializable
    {
        public bool isActivated;
        public override void Deserialize(Packet pPacket)
        {
            isActivated = pPacket.ReadBool();
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isActivated);
        }
    }
}
