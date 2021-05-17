using System;
using System.Collections.Generic;
using System.Text;

namespace sharedCool
{
    public abstract class ASerializable
    {
        //Must Adds to any packet types
        abstract public void Serialize(Packet pPacket);
        abstract public void Deserialize(Packet pPacket);

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
