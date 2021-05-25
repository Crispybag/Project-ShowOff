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

        public Actuator(GameRoom pRoom, int pX, int pY, int _ID, bool activated) : base(pRoom, CollInteractType.SOLID)
        {
            ID = _ID;
            _room = pRoom;
            position[0] = pX;
            position[1] = pY;
            isActivated = activated;
            _room.roomArray[position[0], position[1]].Add(4);
        }
    }

}
