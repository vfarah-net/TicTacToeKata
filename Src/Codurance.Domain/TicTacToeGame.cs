using System;
using System.Collections.Generic;
using System.Text;

namespace Codurance.Domain
{
    public class TicTacToeGame : ITicTacToeGame
    {
        private readonly IBoard board;

        public TicTacToeGame(IBoard board)
        {
            this.board = board;
            Initialize();
        }

        public Action<Dictionary<Team, int>> ScoreChanged { get; set; }
        public Action<Team> TeamChanged { get; set; }

        public Dictionary<Team, int> Scores { get; private set; }

        public void Reset(Team playerOne = Team.Cross)
        {
            board.Reset(playerOne);
        }      

        public bool Move(BoardPosition position)
        {
            return board.Move(position);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"Crosses : {Scores[Team.Cross]}");
            builder.AppendLine($"Zeros : {Scores[Team.Zero]}");
            builder.AppendLine(board.ToString());
            return builder.ToString();
        }

        private void OnGameFinished(GameFinishedArgs gameFinished)
        {
            if(gameFinished.Result == GameResult.Win) {
                int currentScore = Scores[gameFinished.Winner.Team];                
                Scores[gameFinished.Winner.Team] = currentScore + 1;
                ScoreChanged?.Invoke(Scores);
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
            Scores = new Dictionary<Team, int>
            {
                { board.PlayerOne.Team, 0 },
                { board.PlayerTwo.Team, 0 }
            };
        }

    }
}
