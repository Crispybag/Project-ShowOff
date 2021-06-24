using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqResetLevel : ASerializable
    {
        public bool wantsReset = true;
        public string sceneName;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(wantsReset);
            pPacket.Write(sceneName);
        }

        public override void Deserialize(Packet pPacket)
        {
            wantsReset = pPacket.ReadBool();
            sceneName = pPacket.ReadString();
        }
    }
}
