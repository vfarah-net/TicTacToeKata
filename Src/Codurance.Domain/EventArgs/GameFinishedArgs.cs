using System;

namespace Codurance.Domain
{
    public class GameFinishedArgs: EventArgs
    {
        public GameFinishedArgs(
            GameResult result, 
            IPlayer winner = null, 
            LineWin lineWin = LineWin.None)
            : base()
        {            
            LineWin = lineWin;
            Result = result;
            Winner = winner;
        }
        
        public LineWin LineWin { get; }
        public GameResult Result { get; }
        public IPlayer Winner { get; }
    }
}
