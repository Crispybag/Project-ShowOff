using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfReloadScene : ASerializable
    {
        public int playersReset;
        public bool isResetting = false;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(playersReset);    
            pPacket.Write(isResetting);    
        }

        public override void Deserialize(Packet pPacket)
        {
            playersReset = pPacket.ReadInt();
            isResetting = pPacket.ReadBool();
        }
    }
}
