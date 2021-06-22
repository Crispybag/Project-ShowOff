using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqLevelName : ASerializable
    {
        public string levelName;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(levelName);
        }

        public override void Deserialize(Packet pPacket)
        {
            levelName = pPacket.ReadString();
        }
    }
}
