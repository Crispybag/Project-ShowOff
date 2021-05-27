using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{

    //Class for implementing actuators like levers, pressure plate, and buttons.
    //Will contain the information that all actuators can have
    public class Actuator : GameObject
    {
        public bool isActivated = false;
        public int ID;
        public List<int> doors = new List<int>();

        public Actuator(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated = false) : base(pRoom, CollInteractType.SOLID)
        {

        }
    }

}
