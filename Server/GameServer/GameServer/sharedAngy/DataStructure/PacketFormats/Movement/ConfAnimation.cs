using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfAnimation : ASerializable
    {
        public int player;
        public bool isCrawling;
        public bool isFalling;
         

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(player);
            pPacket.Write(isCrawling);
            pPacket.Write(isFalling);
        }

        public override void Deserialize(Packet pPacket)
        {
            player = pPacket.ReadInt();
            isCrawling = pPacket.ReadBool();
            isFalling = pPacket.ReadBool();
        }

    }
}
