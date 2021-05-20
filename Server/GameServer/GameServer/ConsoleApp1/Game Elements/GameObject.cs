using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;


namespace Server
{
    public abstract class GameObject
    {
        public int[] position;
        public enum CollInteractType
        {
            SOLID = 0,
            SHOVE = 1,
            PASS = 2
        }

        protected CollInteractType moveState;

        public GameObject(GameRoom pRoom, CollInteractType pMoveState)
        {
            pRoom.gameObjects.Add(this);
            position = new int[2];
            moveState = pMoveState;
        }

        public virtual void Update()
        {

        }

    }
}
