using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfReloadScene : ASerializable
    {
        bool isResetting = true;
        public string sceneName;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isResetting);
            pPacket.Write(sceneName);
        }

        public override void Deserialize(Packet pPacket)
        {
            isResetting = pPacket.ReadBool();
            sceneName = pPacket.ReadString();
        }
    }
}
