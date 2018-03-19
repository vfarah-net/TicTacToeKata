using System;
using System.Collections.Generic;

namespace Codurance.Domain
{
    public interface IBoard
    {
        IPlayer ActivePlayer { get; }
        IReadOnlyList<Team> BoardPositions { get; }
        IPlayer PlayerOne { get; }
        IPlayer PlayerTwo { get; }

        Action<GameFinishedArgs> GameFinished { get; set; }
        Action<PlayerSwappedArgs> PlayerSwapped { get; set; }

        void Reset(Team playerOneTeam = Team.Zero);
        bool Move(BoardPosition position);
        string ToString();
    }
}