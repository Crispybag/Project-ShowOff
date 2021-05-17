using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqJoinServer : ASerializable
    {
        public string requestedName;


        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(requestedName);
        }

        public override void Deserialize(Packet pPacket)
        {
            requestedName = pPacket.ReadString();
        }
    }
}
