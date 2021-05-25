using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfDoorToggle : ASerializable
    {
        public bool isActivated;
        public int ID;
        public int posX;
        public int posY;
        public int posZ;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isActivated);
            pPacket.Write(ID);
            pPacket.Write(posX);
            pPacket.Write(posY);
            pPacket.Write(posZ);
        }

        public override void Deserialize(Packet pPacket)
        {
            isActivated = pPacket.ReadBool();
            ID = pPacket.ReadInt();
            posX = pPacket.ReadInt();
            posY = pPacket.ReadInt();
            posZ = pPacket.ReadInt();
        }


    }
}
