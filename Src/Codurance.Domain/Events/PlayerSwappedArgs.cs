using System;

namespace Codurance.Domain
{
    public class PlayerSwappedArgs : EventArgs
    {
        public PlayerSwappedArgs(IPlayer currentPlayer)
        {
            CurrentPlayer = currentPlayer;
        }

        public IPlayer CurrentPlayer { get; }
    }
}
