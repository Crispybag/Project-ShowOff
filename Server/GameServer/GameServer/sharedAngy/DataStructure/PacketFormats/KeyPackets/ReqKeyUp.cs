using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ReqKeyUp : ASerializable
    {
        public enum KeyType
        {
            UP = 0,
            DOWN = 1,
            LEFT = 2,
            RIGHT = 3,
            INTERACTION = 4
        }
        public KeyType keyInput;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)keyInput);
        }
        public override void Deserialize(Packet pPacket)
        {
            keyInput = (KeyType)pPacket.ReadInt();    
        }
    }
}
