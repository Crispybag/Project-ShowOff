using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfPlayerSwitch : ASerializable
    {
        public int playerIndex;
        public string playerName;
        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(playerIndex);
            pPacket.Write(playerName);
        }

        public override void Deserialize(Packet pPacket)
        {
            playerIndex = pPacket.ReadInt();
            playerName = pPacket.ReadString();
        }
    }
}