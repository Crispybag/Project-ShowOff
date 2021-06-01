using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    /// <summary>
    /// (Ezra) This class implements a button which activates after interacting with it, turns itself off after a while
    /// </summary>
    class Button : Actuator
    {
        public List<int> elevators = new List<int>();
        public Direction currentDirection;
        //this timer will decide after how long the button will turn off again, 3 stays on for around 0.5 seconds
        public float timer = 3;
        private float currentTimer;

        public Button(GameRoom pRoom, int pX, int pY, int pZ, int pID) : base(pRoom, pX, pY, pZ, pID, CollInteractType.SOLID)
        {
            ID = pID;
            room = pRoom;
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 8;
        }

        public override void Update()
        {
            base.Update();
            //will only run when its activated, since it only needs to turn off after a while
            if (isActivated)
            {
                if (currentTimer <= 0)
                {
                    //send the new package to the players that it turned off.
                    isActivated = false;
                    ConfActuatorToggle newButtonToggle = new ConfActuatorToggle();
                    newButtonToggle.isActived = isActivated;
                    newButtonToggle.ID = ID;
                    newButtonToggle.obj = ConfActuatorToggle.Object.BUTTON;
                    room.sendToAll(newButtonToggle);
                }
                else
                {
                    currentTimer--;
                }
            }
        }

        /// <summary>
        /// (Ezra) Activates button, and updates all elevators
        /// </summary>
        public void SetActive()
        {
            if (!isActivated)
            {
                //the button just turned on, so create new package to send to players.
                isActivated = true;
                currentTimer = timer;

                ConfActuatorToggle newButtonToggle = new ConfActuatorToggle();
                newButtonToggle.isActived = isActivated;
                newButtonToggle.ID = ID;
                newButtonToggle.obj = ConfActuatorToggle.Object.BUTTON;
                room.sendToAll(newButtonToggle);
                UpdateElevators();
            }
        }

        private void UpdateElevators()
        {
            if (elevators.Count > 0)
            {
                foreach (int elevator in elevators)
                {
                    try
                    {
                        (room.InteractableGameobjects[elevator] as Elevator).NextPosition(currentDirection);
                    }
                    catch
                    {
                        Logging.LogInfo("Player.cs: Could not handle the button acutator!", Logging.debugState.DETAILED);
                    }
                }
            }
        }

        /// <summary>
        /// (Ezra) Direction the button should move elevator in, 1 = up, 0 = down
        /// </summary>
        public enum Direction
        {
            UP,
            DOWN
        }
    }
}

