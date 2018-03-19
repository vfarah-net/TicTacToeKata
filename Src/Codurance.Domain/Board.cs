using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Codurance.Domain
{
    public class Board : IBoard
    {
        private readonly List<Team> boardPositions
            = new List<Team>
            {
             // top left, top middle, top right
                Team.None, Team.None, Team.None,
             // middle left, middle, middle right
                Team.None, Team.None, Team.None,
             // bottom left, bottom middle, bottom right
                Team.None, Team.None, Team.None
            };

        private readonly List<ValueTuple<BoardPosition[], LineWin>> winningPositions
            = new List<ValueTuple<BoardPosition[], LineWin>>
        {
                // Horizontal Positions
                {(new [] { BoardPosition.TopLeft, BoardPosition.TopMiddle, BoardPosition.TopRight }, LineWin.TopHorizontal)},
                {(new [] { BoardPosition.MiddleLeft, BoardPosition.Middle, BoardPosition.MiddleRight}, LineWin.MiddleHorizontal)},
                {(new [] { BoardPosition.BottomLeft, BoardPosition.BottomMiddle, BoardPosition.BottomRight}, LineWin.BottomHorizontal)},
                // Vertical Positions
                {(new [] { BoardPosition.TopLeft, BoardPosition.MiddleLeft, BoardPosition.BottomLeft }, LineWin.LeftVertical)},
                {(new [] { BoardPosition.TopMiddle, BoardPosition.Middle, BoardPosition.BottomMiddle }, LineWin.MiddleVertical)},
                {(new [] { BoardPosition.TopRight, BoardPosition.MiddleRight, BoardPosition.BottomRight }, LineWin.RightVertical)},
                // Diagonal Positions
                {(new [] { BoardPosition.TopLeft, BoardPosition.Middle, BoardPosition.BottomRight }, LineWin.LeftDiagonal)},
                {(new [] { BoardPosition.TopRight, BoardPosition.Middle, BoardPosition.BottomLeft }, LineWin.RightDiagonal)},
        };

        public Board(Team playerOneTeam = Team.Zero)
        {
            Reset(playerOneTeam);
        }

        public IReadOnlyList<Team> BoardPositions { get { return boardPositions; } }
        public IPlayer PlayerOne { get; private set; }
        public IPlayer PlayerTwo { get; private set; }

        public IPlayer ActivePlayer
        {
            get
            {
                return PlayerOne.IsActive ? PlayerOne : PlayerTwo;
            }
        }

        public Action<GameFinishedArgs> GameFinished { get; set; }

        public Action<PlayerSwappedArgs> PlayerSwapped { get; set; }

        public bool AreThereAnyMoreMovesLeft
        {
            get
            {
                return boardPositions.Contains(Team.None);
            }
        }

        public bool Move(BoardPosition position)
        {
            if (boardPositions[(int)position] != Team.None)
            {
                return false;
            }

            boardPositions[(int)position] = ActivePlayer.Team;
            SwitchActivePlayer();
            CheckGameStatus();
            return true;
        }

        public void Reset(Team playerOneTeam = Team.Zero)
        {
            for (int i = (int)(BoardPosition.TopLeft); i <= (int)(BoardPosition.BottomRight); i++)
            {
                boardPositions[i] = Team.None;
            }
            if (playerOneTeam == Team.None)
            {
                playerOneTeam = Team.Zero;
            }
            Team playerTwoTeam = Team.Zero;
            if (playerOneTeam == Team.Zero)
            {
                playerTwoTeam = Team.Cross;
            }

            PlayerOne = new Player(playerOneTeam, playerOneTeam == Team.Zero, nameof(PlayerOne));
            PlayerTwo = new Player(playerTwoTeam, playerOneTeam == Team.Zero, nameof(PlayerTwo));
            PlayerSwapped?.Invoke(new PlayerSwappedArgs(ActivePlayer));
        }

        public override string ToString()
        {
            StringBuilder logger = new StringBuilder();
            logger.AppendLine($"{BoardPositions[0].AsChar()} | {BoardPositions[1].AsChar()} | {BoardPositions[2].AsChar()}");
            logger.AppendLine($"{BoardPositions[3].AsChar()} | {BoardPositions[4].AsChar()} | {BoardPositions[5].AsChar()}");
            logger.AppendLine($"{BoardPositions[6].AsChar()} | {BoardPositions[7].AsChar()} | {BoardPositions[8].AsChar()}");
            return logger.ToString();
        }

        private void SwitchActivePlayer()
        {
            var switchedPlayerOne = new Player(PlayerOne.Team, !PlayerOne.IsActive, PlayerOne.Name);
            var switchedPlayerTwo = new Player(PlayerTwo.Team, !PlayerTwo.IsActive, PlayerTwo.Name);
            PlayerOne = switchedPlayerOne;
            PlayerTwo = switchedPlayerTwo;
            PlayerSwapped?.Invoke(new PlayerSwappedArgs(ActivePlayer));
        }

        private void CheckGameStatus()
        {
            IList<Team> lines = new List<Team>();
            foreach (var winningPosition in winningPositions)
            {
                lines = GetEqualTeamLine(winningPosition.Item1);
                if (lines.Any())
                {
                    OnGameFinished(lines, winningPosition.Item2);
                }
            }

            if (!AreThereAnyMoreMovesLeft)
            {
                OnGameFinished(lines, LineWin.None);
            }
        }

        private void OnGameFinished(IList<Team> winningValues, LineWin lineWin)
        {
            if (winningValues.Any())
            {
                var winner = PlayerOne.Team == winningValues[0] ? PlayerOne : PlayerTwo;
                GameFinished?.Invoke(new GameFinishedArgs(GameResult.Win, winner, lineWin));
                Debug.WriteLine($"Game finished with team '{winner.Team}' winning");
            }
            else
            {
                GameFinished?.Invoke(new GameFinishedArgs(GameResult.Draw));
                Debug.WriteLine("Game finished in a draw");
            }
            Debug.WriteLine(this.ToString());
        }

        private IList<Team> GetEqualTeamLine(params BoardPosition[] positions)
        {
            List<Team> result = new List<Team>();
            foreach (int position in positions)
            {
                result.Add(boardPositions[position]);
            }
            var allEqualAndAValidTeam =
                result[0] != Team.None &&
                result[0] == result[1] &&
                result[1] == result[2];
            return allEqualAndAValidTeam ? result : new List<Team>(0);
        }
    }
}
