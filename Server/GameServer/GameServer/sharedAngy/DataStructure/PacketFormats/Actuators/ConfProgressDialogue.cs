using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfProgressDialogue : ASerializable
    {
        public int ID;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(ID);
        }

        public override void Deserialize(Packet pPacket)
        {
            ID = pPacket.ReadInt();
        }


    }
}
