using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{

    /// <summary>
    /// (Ezra) Class for all actuators, this class holds all necessary information that all actuators have
    /// </summary>
    public class Actuator : GameObject
    {
        public bool isActivated = false;
        public int ID;
        public List<int> doors = new List<int>();

        public Actuator(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated = false) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {

        }
    }

}
