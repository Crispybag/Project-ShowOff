using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class Button : Actuator
    {
        public List<Elevator> elevators = new List<Elevator>();
        public Direction currentDirection;
        public float timer = 3;
        private float currentTimer;

        public Button(GameRoom pRoom, int pX, int pY, int pID) : base(pRoom, pX, pY, pID)
        {
            _room = pRoom;
            ID = pID;
            position[0] = pX;
            position[1] = pY;
            _room.roomArray[position[0], position[1]].Add(8);
            objectIndex = 8;
        }

        public override void Update()
        {
            base.Update();
            if (isActivated)
            {
                if (currentTimer <= 0)
                {
                    isActivated = false;
                    ConfActuatorToggle newButtonToggle = new ConfActuatorToggle();
                    newButtonToggle.isActived = isActivated;
                    newButtonToggle.ID = ID;
                    newButtonToggle.obj = ConfActuatorToggle.Object.BUTTON;
                    _room.sendToAll(newButtonToggle);
                }
                else
                {
                    currentTimer--;
                }
            }
        }

        public void SetActive()
        {
            isActivated = true;
            currentTimer = timer;

            ConfActuatorToggle newButtonToggle = new ConfActuatorToggle();
            newButtonToggle.isActived = isActivated;
            newButtonToggle.ID = ID;
            newButtonToggle.obj = ConfActuatorToggle.Object.BUTTON;
            _room.sendToAll(newButtonToggle);
        }



        public enum Direction
        {
            UP,
            DOWN
        }
    }
}

