using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class HighScores : ASerializable
    {

        public string scores;



        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(scores);
        }

        public override void Deserialize(Packet pPacket)
        {
            scores = pPacket.ReadString();
        }

    }
}
