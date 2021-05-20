using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game_Elements
{
    GameRoom _room;
    public Lever(GameRoom pRoom, int pX, int pY) : base(CollInteractType.SOLID)
    {
        _room = pRoom;
        position[0] = pX;
        position[1] = pY;
        _room.roomArray[position[0], position[1]] = 4;
    }
}
