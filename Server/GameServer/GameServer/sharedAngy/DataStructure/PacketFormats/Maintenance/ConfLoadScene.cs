using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfLoadScene : ASerializable
    {
        public bool isResetting = true;
        public string sceneName;
        public int playersReset;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isResetting);
            pPacket.Write(sceneName);
            pPacket.Write(playersReset);

        }

        public override void Deserialize(Packet pPacket)
        {
            isResetting = pPacket.ReadBool();
            sceneName = pPacket.ReadString();
            playersReset = pPacket.ReadInt();
        }
    }
}
