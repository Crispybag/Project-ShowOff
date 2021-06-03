using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfReloadScene : ASerializable
    {
        bool isResetting = true;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isResetting);    
        }

        public override void Deserialize(Packet pPacket)
        {
            isResetting = pPacket.ReadBool();
        }
    }
}
