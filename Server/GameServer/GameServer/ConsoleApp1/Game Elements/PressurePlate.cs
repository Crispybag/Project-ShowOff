using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class PressurePlate : Actuator
    {
        public PressurePlate(GameRoom pRoom, int pX, int pY, int pID, bool pActivated) : base(pRoom, pX, pY, pID, pActivated)
        {
            _room = pRoom;
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            isActivated = pActivated;
            _room.roomArray[position[0], position[1]].Add(5);
            objectIndex = 5;
        }

        private bool newActived;


        public override void Update()
        {
            base.Update();
            if(_room.roomArray[position[0], position[1]].Count > 0)
            {
                List<int> allItems = _room.roomArray[position[0], position[1]];
                foreach(int item in allItems)
                {
                    //1 = player, 7 = box
                    if(item == 1 || item == 7)
                    {
                        newActived = true;
                        break;
                    }
                    else
                    {
                        newActived = false;
                    }
                }
                if (newActived != isActivated)
                {
                    UpdateClient();
                }
            }


        }

        private void UpdateClient()
        {
            isActivated = newActived;
            Logging.LogInfo("Pressure Plate: " + isActivated, Logging.debugState.DETAILED);
            ConfActuatorToggle plateToggle = new ConfActuatorToggle();
            plateToggle.ID = ID;
            plateToggle.isActived = isActivated;
            plateToggle.obj = ConfActuatorToggle.Object.PRESSUREPLATE;
            _room.sendToAll(plateToggle);
        }


    }
}
