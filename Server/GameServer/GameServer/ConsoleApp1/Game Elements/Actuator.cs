using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Actuator : GameObject
    {
        public GameRoom _room;
        public bool isActivated = false;
        public int ID;
        public List<Door> doors = new List<Door>();

        public Actuator(GameRoom pRoom, int pX, int pY, int pID, bool pActivated) : base(pRoom, CollInteractType.SOLID)
        {

        }
    }

}
