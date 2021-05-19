using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfJoinRoom : ASerializable
    {
        public Rooms room;

        public override void Serialize(Packet pPacket)
        {
            pPacket.Write((int)room);
        }

        public override void Deserialize(Packet pPacket)
        {
            room = (Rooms)pPacket.ReadInt();
        }

        public enum Rooms
        {
            LOGIN = 0,
            LOBBY = 1,
            GAME = 2
        }

    }
}
