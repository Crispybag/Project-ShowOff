using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{

    class PressurePlate : Actuator
    {

        public List<Door> doors = new List<Door>();
        private bool newActived;

        public PressurePlate(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated) : base(pRoom, pX, pY, pZ, pID, pActivated)
        {
            room = pRoom;
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            isActivated = pActivated;
            room.roomArray[position[0], position[1], position[2]].Add(5);
            objectIndex = 5;
        }




        public override void Update()
        {
            base.Update();
            if(room.roomArray[position[0], position[1], position[2]].Count > 0)
            {
                List<int> allItems = room.roomArray[position[0], position[1], position[2]];
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
                //This check makes sure the player doesnt contantly get packages of the pressureplate being updated.
                if (newActived != isActivated)
                {
                    UpdateClient();
                }
            }


        }

        private void UpdateClient()
        {
            isActivated = newActived;
            ConfActuatorToggle plateToggle = new ConfActuatorToggle();
            plateToggle.ID = ID;
            plateToggle.isActived = isActivated;
            plateToggle.obj = ConfActuatorToggle.Object.PRESSUREPLATE;
            room.sendToAll(plateToggle);
            foreach(Door door in doors)
            {
                door.CheckDoor();
            }
        }


    }
}
