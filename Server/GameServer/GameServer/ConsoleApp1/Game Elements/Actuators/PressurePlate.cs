using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Subclass of actuator for implementing a pressureplate.
    /// </summary>
    class PressurePlate : Actuator
    {

        public List<int> doors = new List<int>();
        private bool newActivated;

        public PressurePlate(GameRoom pRoom, int pX, int pY, int pZ, int pID, bool pActivated, List<int> pRedstones) : base(pRoom, pX, pY, pZ, pID, CollInteractType.PASS, pRedstones, pActivated)
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
                        newActivated = true;
                        break;
                    }
                    else
                    {
                        newActivated = false;
                    }
                }

                //This check makes sure the player doesnt contantly get packages of the pressureplate being updated.
                if (newActivated != isActivated)
                {
                    OnInteract(0, ConfActuatorToggle.Object.PRESSUREPLATE);
                }
            }
        }
    }
}
