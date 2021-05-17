using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfJoinServer : ASerializable
    {
        public bool acceptStatus;


        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(acceptStatus);
        }

        public override void Deserialize(Packet pPacket)
        {
            acceptStatus = pPacket.ReadBool();
        }
    }
}