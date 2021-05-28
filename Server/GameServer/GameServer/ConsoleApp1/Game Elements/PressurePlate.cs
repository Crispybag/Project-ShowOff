﻿using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{

    class PressurePlate : Actuator
    {

        public List<int> doors = new List<int>();
        private bool newActived;

        public PressurePlate(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated) : base(pRoom, pX, pY, pZ, pID, pActivated)
        {
            room = pRoom;
            ID = pID;
            isActivated = pActivated;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 5;
        }




        public override void Update()
        {
            base.Update();
            if(room.roomArray[x(), y(), z()].Count > 0)
            {
                List<GameObject> allItems = room.roomArray[x(), y(), z()];
                foreach(GameObject item in allItems)
                {
                    //1 = player, 7 = box
                    if(item.objectIndex == 1 || item.objectIndex == 7)
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
            foreach(int door in doors)
            {
                try
                {
                    (room.InteractableGameobjects[door] as Door).CheckDoor();
                }
                catch
                {
                    Logging.LogInfo("PressurePlate.cs: Could not handle given information about door, probably because its not in interactable list!", Logging.debugState.DETAILED);
                }
            }
        }


    }
}
