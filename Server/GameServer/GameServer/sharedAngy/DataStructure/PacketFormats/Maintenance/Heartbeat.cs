using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    //AUTHOR: Leo
    //DESCRIPTION: sends out a heartbeat to keep an eye on weird clients
    public class Heartbeat : ASerializable
    {
        public int puls;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(0);
        }

        public override void Deserialize(Packet pPacket)
        {
            puls = pPacket.ReadInt();
        }
    }
}
