using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class SpawnPoint : GameObject
    {
        public int spawnIndex;
        public SpawnPoint(GameRoom pRoom, int pX, int pY, int pZ, int pPlayer) : base(pX, pY, pZ, pRoom, CollInteractType.PASS)
        {
            spawnIndex = pPlayer;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            room.spawnPoints.Add(this);

            objectIndex = 3;
        }
    }
}
