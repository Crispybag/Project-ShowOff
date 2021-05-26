using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfActuatorToggle : ASerializable
    {
        public bool isActived;
        public int ID;
        public Object obj;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(isActived);
            pPacket.Write(ID);
            pPacket.Write((int)obj);
        }

        public override void Deserialize(Packet pPacket)
        {
            isActived = pPacket.ReadBool();
            ID = pPacket.ReadInt();
            obj = (Object)pPacket.ReadInt();
        }

        public enum Object
        {
            LEVER,
            PRESSUREPLATE,
            BUTTON
        }

    }
}
