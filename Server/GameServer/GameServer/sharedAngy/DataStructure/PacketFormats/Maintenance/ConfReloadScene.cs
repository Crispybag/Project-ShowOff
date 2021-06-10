using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfReloadScene : ASerializable
    {
        bool isResetting = true;
        public string sceneName;
        public bool isResetting = false;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isResetting);
            pPacket.Write(sceneName);
            pPacket.Write(playersReset);    

        }

        public override void Deserialize(Packet pPacket)
        {
            isResetting = pPacket.ReadBool();
            playersReset = pPacket.ReadInt();
            sceneName = pPacket.ReadString();
        }
    }
}
