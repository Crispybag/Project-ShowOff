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
    }
}
