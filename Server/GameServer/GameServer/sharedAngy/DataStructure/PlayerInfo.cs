using System;

namespace sharedAngy
{
    public class PlayerInfo
    {
        private string _playerName;
        private int playerIndex;
        //first initialise
        public PlayerInfo(string pName, int pPlayerIndex)
        {
            _playerName = pName;
            playerIndex = pPlayerIndex;
        }

        //Get the player's name
        public string GetPlayerName()
        {
            return _playerName;
        }

        public int GetPlayerIndex()
        {
            return playerIndex;
        }

        public void SetPlayerIndex(int i)
        {
            if (i == 0) playerIndex = i;
            else if (i == 1) i = 1;
            else Logging.LogInfo("Trying to set a player index to something that is not 1 with SetPlayerIndex");
        }
    }
}
