using System;
using System.Collections.Generic;

namespace Codurance.Domain
{
    public interface ITicTacToeGame
    {
        Action<Dictionary<Team, int>> ScoreChanged { get; set; }
        Action<Team> TeamChanged { get; set; }
        Dictionary<Team, int> Scores { get; }

        bool Move(BoardPosition position);
        void Reset(Team playerOne = Team.Cross);
    }
}