using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
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
