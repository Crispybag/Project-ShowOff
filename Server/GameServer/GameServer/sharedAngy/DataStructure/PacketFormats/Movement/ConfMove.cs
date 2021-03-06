using System;
using System.Collections.Generic;
using System.Text;

namespace sharedAngy
{
    public class ConfMove : ASerializable
    {
        public int player;
        public int dirX;
        public int dirY;
        public int dirZ;
        public int orientation;
        public bool isFalling = false;

        public int calls;
        public string directions;


        public override void Serialize(Packet pPacket)
        {
            pPacket.Write(player);
            pPacket.Write(dirX);
            pPacket.Write(dirY);
            pPacket.Write(dirZ);
            pPacket.Write(orientation);
            pPacket.Write(isFalling);

            pPacket.Write(calls);
            pPacket.Write(directions);
        }

        public override void Deserialize(Packet pPacket)
        {
            player = pPacket.ReadInt();
            dirX = pPacket.ReadInt();
            dirY = pPacket.ReadInt();
            dirZ = pPacket.ReadInt();
            orientation = pPacket.ReadInt();
            isFalling = pPacket.ReadBool();

            calls = pPacket.ReadInt();
            directions = pPacket.ReadString();
        }
    }
}
