using System;
using System.Collections.Generic;
using System.Text;

namespace Codurance.Domain
{
    public class TicTacToeGame : ITicTacToeGame
    {
        private readonly IBoard board;
        private Dictionary<Team, int> scores;

        public TicTacToeGame(IBoard board)
        {
            this.board = board;
            Initialize();
        }

        public Action<IReadOnlyDictionary<Team, int>> ScoreChanged { get; set; }
        public Action<Team> TeamChanged { get; set; }
        public Action<ValueTuple<Team, LineWin>> TeamAndLineWon { get; set; }

        public IReadOnlyDictionary<Team, int> Scores { get { return scores; }}

        public bool IsGameOver { get; private set; }

        public void Reset()
        {
            board.Reset();
            IsGameOver = false;
        }      

        public bool Move(BoardPosition position)
        {
            return board.Move(position);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Crosses : {Scores[Team.Cross]}");
            builder.AppendLine($"Zeros : {Scores[Team.Zero]}");
            builder.AppendLine(board.ToString());
            return builder.ToString();
        }

        private void OnGameFinished(GameFinishedArgs gameFinished)
        {
            IsGameOver = true;
            if (gameFinished.Result == GameResult.Win) {
                int currentScore = Scores[gameFinished.Winner.Team];                
                scores[gameFinished.Winner.Team] = currentScore + 1;
                ScoreChanged?.Invoke(Scores);
                TeamAndLineWon?.Invoke(new ValueTuple<Team, LineWin>(gameFinished.Winner.Team, gameFinished.LineWin));
            }
            else
            {
                TeamAndLineWon?.Invoke(new ValueTuple<Team, LineWin>(Team.None, LineWin.None));
            }
        }

        private void OnPlayerSwapped(PlayerSwappedArgs playerSwapped)
        {
            TeamChanged?.Invoke(playerSwapped.CurrentPlayer.Team);
        }

        private void Initialize()
        {
            board.GameFinished += OnGameFinished;
            board.PlayerSwapped += OnPlayerSwapped;
            scores = new Dictionary<Team, int>
            {
                { board.PlayerOne.Team, 0 },
                { board.PlayerTwo.Team, 0 }
            };
        }
    }
}
