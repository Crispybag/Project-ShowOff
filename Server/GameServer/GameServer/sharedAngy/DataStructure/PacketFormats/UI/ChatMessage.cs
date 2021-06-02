using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ChatMessage : ASerializable
    {

        public string textMessage;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(textMessage);
        }
        public override void Deserialize(Packet pPacket)
        {
            textMessage = pPacket.ReadString();
        }


    }
}
