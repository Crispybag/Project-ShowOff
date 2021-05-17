using System;

namespace sharedCool
{
    public class PlayerInfo
    {
        private string _playerName;

        //first initialise
        public PlayerInfo(string pName)
        {
            _playerName = pName;
        }

        //Get the player's name
        public string GetPlayerName()
        {
            return _playerName;
        }
    }
}
