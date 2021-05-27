using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class SpawnPoint : GameObject
    {
        public int spawnIndex;
        public SpawnPoint(GameRoom pRoom, int pX, int pY, int pZ, int pPlayer) : base(pRoom, CollInteractType.PASS)
        {
            spawnIndex = pPlayer;
            room = pRoom;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            room.roomArray[position[0], position[1], position[2]].Add(3);
            objectIndex = 3;
        }
    }
}
