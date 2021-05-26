using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;


namespace Server
{
    public abstract class GameObject
    {
        //position that each game object hasa
        public int[] position;
        public int objectIndex = 0;
        //determines how a collision interaction will be handled
        public enum CollInteractType
        {
            SOLID = 0,
            SHOVE = 1,
            PASS = 2
        }

        
        protected CollInteractType moveState;


        //First initialise
        public GameObject(GameRoom pRoom, CollInteractType pMoveState)
        {
            //Add gameobject to the list of the room it is a part of so it gets updated once the scene is loaded
            pRoom.gameObjects.Add(this);

            //initialise values
            position = new int[3];
            moveState = pMoveState;
        }

        protected void SetState(CollInteractType pNewState)
        {
            moveState = pNewState;
        }


        //override update void for children to use
        public virtual void Update()
        {

        }

    }
}
