using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) Sub class of actuator, because it has very similar functionalities to actuator, after activated once, it has been fixed.
    /// </summary>
    class Dialogue : GameObject
    {
        public int ID;
        public List<GameObject> dialogueHitBoxes = new List<GameObject>();

        public Dialogue(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pX, pY, pZ, pRoom, CollInteractType.PASS)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
        }

        public void DestroyDialogue()
        {
            foreach(DialogueHitBoxes dialogue in dialogueHitBoxes)
            {
                room.OnCoordinatesRemove(dialogue.x(), dialogue.y(), dialogue.z(), dialogue.objectIndex);
            }
            room.OnCoordinatesRemove(x(), y(), z(), objectIndex);
        }

    }
}
