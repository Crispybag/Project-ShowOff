using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class DialogueHitBoxes : GameObject
    {
        public Dialogue parentDialogue;
        public DialogueHitBoxes(GameRoom pRoom, int pX, int pY, int pZ) : base(pX, pY, pZ, pRoom, CollInteractType.PASS)
        {
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 15;
        }
    }
}
