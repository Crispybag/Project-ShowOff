using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{

    /// <summary>
    /// (Ezra) Class for all actuators, this class holds all necessary information that all actuators have
    /// </summary>
    public abstract class Actuator : GameObject
    {
        public bool isActivated = false;
        public int ID;
        public List<int> redstoneOutputs = new List<int>();

        public Actuator(GameRoom pRoom, int pX, int pY, int pZ, int pID, CollInteractType collType, List<int> pRedstones, bool pActivated = false) : base(pX, pY, pZ, pRoom, collType)
        {
            ID = pID;
            isActivated = pActivated;

            //add redstone outputs here
            try
            {
                foreach (int index in pRedstones)
                {
                    Logging.LogInfo("GameRoom.cs: Added door to button!", Logging.debugState.DETAILED);
                    redstoneOutputs.Add(index);
                }
            }
            catch
            {
                Logging.LogInfo("GameRoom.cs: We could not handle given information about button", Logging.debugState.DETAILED);
            }
        }

        protected void ToggleActuator(ConfActuatorToggle.Object type)
        {
            ConfActuatorToggle actuatorToggle = new ConfActuatorToggle();
            actuatorToggle.ID = ID;
            actuatorToggle.isActived = isActivated;
            actuatorToggle.obj = type;
            room.sendToAll(actuatorToggle);
        }



        public virtual void OnInteract(ConfActuatorToggle.Object pType = ConfActuatorToggle.Object.UNINDENTIFIED)
        {
            if (pType == ConfActuatorToggle.Object.UNINDENTIFIED)
            {
                Logging.LogInfo("Caution, an unindentified actuator call has been called, make sure to fix this as it might not respond correctly in the client", Logging.debugState.DETAILED);
            }

            isActivated = !isActivated;
            ToggleActuator(pType);

            foreach (int output in redstoneOutputs)
            {
                int removethisWhenyouseeit = redstoneOutputs.Count;
                try
                {
                    if (room.InteractableGameobjects[output] is RedstoneOutput) (room.InteractableGameobjects[output] as RedstoneOutput).CheckOutput();
                }
                catch
                {
                    Logging.LogInfo("Lever.cs Could not handle door, probably not in interactablegameobject list in room!", Logging.debugState.DETAILED);
                }
            }
        }


    }

}
