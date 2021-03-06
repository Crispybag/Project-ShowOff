using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;


namespace Server
{
    /// <summary>
    /// (Leo) Class for implementing all objects which need positions and functions
    /// </summary>
    public abstract class GameObject
    {
        //position that each game object hasa
        private int[] position;
        public int x() { return position[0]; }
        public int y() { return position[1]; }
        public int z() { return position[2]; }

        private bool isCurrentlyInAirChannel = false;


        public int objectIndex = 0;
        //determines how a collision interaction will be handled
        public enum CollInteractType
        {
            SOLID = 0,
            SHOVE = 1,
            PASS = 2
        }

        
        public CollInteractType collState;

        public GameRoom room;
        //First initialise
        public GameObject(int pX, int pY, int pZ, GameRoom pRoom, CollInteractType pMoveState)
        {
            //Add gameobject to the list of the room it is a part of so it gets updated once the scene is loaded
            pRoom.gameObjects.Add(this);
            room = pRoom;
            //initialise values
            position = new int[3];
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;
            collState = pMoveState;
        }

        public void SetState(CollInteractType pNewState)
        {
            collState = pNewState;
        }

        #region movement tools
        public void MoveDirection(int pX, int pY, int pZ, bool isAirChanneling = false, int playerIndex = 1)
        {
            //change the location of the player and remove the player value from the grid at the place the player was
            room.OnCoordinatesRemove(x(), y(), z(), objectIndex);
            position[0] += pX;
            position[1] += pY;
            position[2] += pZ;

            SendAirChannelAnimation(isAirChanneling, playerIndex);

            //add
            room.roomArray[x(), y(), z()].Add(this);
            //Logging.LogInfo("Player's position is now ( " + position[0] + ", " + position[1] + ", " + position[2] + ")", Logging.debugState.DETAILED);
        }
        public void MoveDirection(int[] pDirection, bool isAirChanneling = false, int playerIndex = 1)
        {
            try
            {
                //change the location of the player and remove the player value from the grid at the place the player was
                room.OnCoordinatesRemove(x(), y(), z(), objectIndex);
                position[0] += pDirection[0];
                position[1] += pDirection[1];
                position[2] += pDirection[2];
                //add
                SendAirChannelAnimation(isAirChanneling, playerIndex);

                room.roomArray[x(), y(), z()].Add(this);
                Logging.LogInfo("Player's position is now ( " + position[0] + ", " + position[1] + ", " + position[2] + ")", Logging.debugState.SPAM);
            }
            catch
            {
                Logging.LogInfo("index was outside of bounds in move direction in gameobject", Logging.debugState.SIMPLE);
            }
        }



        public void MovePosition(int pX, int pY, int pZ, bool isAirChanneling = false, int playerIndex = 1)
        {
            //change the location of the player and remove the player value from the grid at the place the player was
            room.OnCoordinatesRemove(x(), y(), z(), objectIndex);
            position[0] = pX;
            position[1] = pY;
            position[2] = pZ;

            SendAirChannelAnimation(isAirChanneling, playerIndex);

            //add
            room.roomArray[x(), y(), z()].Add(this);
            Logging.LogInfo("Player's position is now ( " + position[0] + ", " + position[1] + ", " + position[2] + ")", Logging.debugState.SPAM);
        }




        public void MovePosition(int[] pPosition, bool isAirChanneling = false, int playerIndex = 1)
        {
            try
            {
                //change the location of the player and remove the player value from the grid at the place the player was
                room.OnCoordinatesRemove(x(), y(), z(), objectIndex);
                position[0] = pPosition[0];
                position[1] = pPosition[1];
                position[2] = pPosition[2];

                SendAirChannelAnimation(isAirChanneling, playerIndex);

                //add
                room.roomArray[x(), y(), z()].Add(this);
                Logging.LogInfo("Player's position is now ( " + position[0] + ", " + position[1] + ", " + position[2] + ")", Logging.debugState.SPAM);
            }
            catch
            {
                Logging.LogInfo("index was outside of bounds in move direction in gameobject", Logging.debugState.SIMPLE);
            }
        }
        #endregion

        //override update void for children to use
        public virtual void Update()
        {

        }

        private void SendAirChannelAnimation(bool isAirChanneling, int playerIndex)
        {

            if (isCurrentlyInAirChannel != isAirChanneling)
            {
                ConfAnimation animation = new ConfAnimation();
                animation.player = playerIndex;
                animation.isInAirChannel = isAirChanneling;
                room.sendToAll(animation);
                isCurrentlyInAirChannel = isAirChanneling;
                Console.WriteLine("Send package with the value of: " + isAirChanneling);
            }
        }


    }
}
