using System;
using System.Collections.Generic;

namespace Codurance.Domain
{
    public interface ITicTacToeGame
    {
        bool IsGameOver { get; }
        Action<IReadOnlyDictionary<Team, int>> ScoreChanged { get; set; }
        Action<Team> TeamChanged { get; set; }
        Action<ValueTuple<Team, LineWin>> TeamAndLineWon { get; set; }
        IReadOnlyDictionary<Team, int> Scores { get; }

        bool Move(BoardPosition position);
        void Reset(Team playerOne = Team.Cross);
        string ToString();
    }
}