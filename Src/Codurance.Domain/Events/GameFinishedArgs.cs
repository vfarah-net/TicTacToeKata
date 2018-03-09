using System;

namespace Codurance.Domain
{
    public class GameFinishedArgs: EventArgs
    {
        public GameFinishedArgs(GameResult result, IPlayer winner = null)
            : base()
        {
            Winner = winner;
            Result = result;
        }

        public IPlayer Winner { get; }
        public GameResult Result { get; }
    }
}
