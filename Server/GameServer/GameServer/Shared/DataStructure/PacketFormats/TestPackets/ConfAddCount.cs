using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class ConfAddCount : ASerializable
    {
        public int totalCount;
        public override void Deserialize(Packet pPacket)
        {
            totalCount = pPacket.ReadInt();
        }

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(totalCount);
        }
    }
}
