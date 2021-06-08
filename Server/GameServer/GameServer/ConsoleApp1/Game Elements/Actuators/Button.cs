﻿using System;
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
        public Direction currentDirection;
        //this timer will decide after how long the button will turn off again, 3 stays on for around 0.5 seconds
        public float timer = 3;
        private float currentTimer;

        public Button(GameRoom pRoom, int pX, int pY, int pZ, int pID, List<int> pRedstones) : base(pRoom, pX, pY, pZ, pID, CollInteractType.SOLID, pRedstones)
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

                     base.OnInteract(ConfActuatorToggle.Object.BUTTON);
                    
                }
                else
                {
                    currentTimer--;
                }
            }
        }

        public override void OnInteract(ConfActuatorToggle.Object pType = ConfActuatorToggle.Object.BUTTON)
        {
            if (!isActivated)
            {
                base.OnInteract(pType);
                currentTimer = timer;
                UpdateElevators();
            }

        }



        private void UpdateElevators()
        {
            Console.WriteLine("Elevator count: " + redstoneOutputs.Count);
            if (redstoneOutputs.Count > 0)
            {
                foreach (int elevator in redstoneOutputs)
                {
                    Console.WriteLine( "current check in button : " + elevator + " : "+ room.InteractableGameobjects[elevator]);
                    try
                    {
                        if (room.InteractableGameobjects[elevator] is Elevator)
                        {
                        (room.InteractableGameobjects[elevator] as Elevator).NextPosition(currentDirection);
                        }

                        else if (room.InteractableGameobjects[elevator] is WaterPool)
                        {
                            (room.InteractableGameobjects[elevator] as WaterPool).moveWater((int)currentDirection);
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Logging.LogInfo("Button.cs: Could not handle the button acutator!", Logging.debugState.DETAILED);
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

