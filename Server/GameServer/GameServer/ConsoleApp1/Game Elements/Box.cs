using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;
namespace Server
{
    /// <summary>
    /// (Leo) Contains the information about the box
    /// </summary>
    public class Box : GameObject
    {
        private bool containsWater;
        public int ID;

        public Box(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pX, pY, pZ, pRoom, CollInteractType.SOLID)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 7;
        }

        public bool tileContainsWater(int pID)
        {
            List<GameObject> objectsOnCoord = room.OnCoordinatesGetGameObjects(x(), y(), z() );
            int i = 0;
            containsWater = false;
            foreach (GameObject index in objectsOnCoord)
            {
                i++;
                if(index.collState == CollInteractType.SOLID && index.objectIndex != 16 && index.objectIndex != 7 && pID != this.ID)
                {
                    return false;
                }
                else if(index.objectIndex == 16)
                {
                    containsWater = true;
                }
                if(i == objectsOnCoord.Count && containsWater)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
