using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfPlayer : ASerializable
    {

        public string playerName;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(playerName);   
        }

        public override void Deserialize(Packet pPacket)
        {
            playerName = pPacket.ReadString();
        }


    }
}
