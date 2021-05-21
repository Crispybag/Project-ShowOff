using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class SpawnPoint : GameObject
    {
        GameRoom _room;
        public int spawnIndex;
        public SpawnPoint(GameRoom pRoom, int pPlayer, int pX, int pY) : base(pRoom, CollInteractType.PASS)
        {
            spawnIndex = pPlayer;
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(3);
            objectIndex = 3;
        }
    }
}
