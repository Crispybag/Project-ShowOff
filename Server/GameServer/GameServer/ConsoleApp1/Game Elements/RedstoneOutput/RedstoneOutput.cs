using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Class for all actuators, this class holds all necessary information that all actuators have
    /// </summary>
    public abstract class RedstoneOutput : GameObject
    {
        public bool isActivated = false;
        public int ID;
        public List<int> actuators = new List<int>();

        public RedstoneOutput(GameRoom pRoom, int pX, int pY, int pZ, int pID, CollInteractType collType, List<int> pActuators, bool pActivated = false) : base(pX, pY, pZ, pRoom, collType)
        {
            ID = pID;
            isActivated = pActivated;

            try
            {
                foreach (int index in pActuators)
                {
                    Logging.LogInfo("GameRoom.cs: Added actuator to redstone output", Logging.debugState.DETAILED);
                    actuators.Add(index);
                }
            }

            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about redstone output", Logging.debugState.DETAILED);
            }

        }

        public virtual void CheckOutput()
        {
            isActivated = checkActuators();
        }

        private bool checkActuators()
        {
            foreach (int actuator in actuators)
            {
                try
                {
                    if (!(room.InteractableGameobjects[actuator] as Actuator).isActivated)
                    {
                        return false;
                    }
                }
                catch
                {
                    Logging.LogInfo("Airchannel.cs: Could not handle actuator, probably not in list in room!", Logging.debugState.DETAILED);
                }
            }
            return true;
        }


    }
}
