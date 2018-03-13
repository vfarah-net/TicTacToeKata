using FluentAssertions;
using NUnit.Framework;

namespace Codurance.Domain.Unit.Tests
{
    [TestFixture]
    public class WhenUsingTheBoardToPlayTicTacToe
    {
        [TestFixture]
        public class AndCreatingANewBoard : WhenUsingTheBoardToPlayTicTacToe
        {

            [TestCase(Team.Cross)]
            [TestCase(Team.Zero)]
            public void ShouldHaveTwoPlayersOnDifferentTeamsWithACrossOrZeroTeam(Team teamState)
            {
                IBoard subject = new Board(teamState);

                subject.PlayerOne.Should().NotBeNull();
                subject.PlayerTwo.Should().NotBeNull();
                subject.PlayerOne.Team.Should().NotBe(subject.PlayerTwo.Team);
                subject.PlayerOne.Team.Should().NotBe(Team.None);
                subject.PlayerTwo.Team.Should().NotBe(Team.None);
            }

            [Test]
            public void ShouldMakeThePlayerWithTeamZeroTheFirstPlayerToMove()
            {
                IBoard subject = new Board();

                subject.ActivePlayer.Should().NotBeNull();
                subject.ActivePlayer.Team.Should().Be(Team.Zero);
                subject.ActivePlayer.IsActive.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndMakingMovesOnTheBoard : WhenUsingTheBoardToPlayTicTacToe
        {
            [Test]
            public void ShouldSwapPlayerForEverySuccessfullMove()
            {
                IBoard subject = new Board();
                var activePlayer = subject.ActivePlayer;

                var actual = subject.Move(BoardPosition.TopLeft);

                actual.Should().BeTrue();
                activePlayer.Should().NotBe(subject.ActivePlayer);
            }

            [Test]
            public void ShouldRaiseAPlayerSwappedEventForSuccessfullMove()
            {
                IBoard subject = new Board();
                IPlayer currentPlayer = null;
                subject.PlayerSwapped += (PlayerSwappedArgs args) =>
                {
                    currentPlayer = args.CurrentPlayer;
                };
                var initialPlayer = subject.ActivePlayer;
                var actual = subject.Move(BoardPosition.TopLeft);

                currentPlayer.Should().NotBeNull();
                initialPlayer.Should().NotBe(currentPlayer);
                currentPlayer.Should().Be(subject.ActivePlayer);
            }

            [Test]
            public void ShouldFailWhenTryingToMoveToAPositionThatAlreadyHasAValue()
            {
                IBoard subject = new Board();

                var actual = subject.Move(BoardPosition.TopLeft);
                var actualMoveInTheSameCorner = subject.Move(BoardPosition.TopLeft);

                actual.Should().BeTrue();
                actualMoveInTheSameCorner.Should().BeFalse();
            }

            [Test]
            public void ShouldNotSwapUsersWhenAFailedMoveOccurs()
            {
                IBoard subject = new Board();
                var actual = subject.Move(BoardPosition.TopLeft);
                var activePlayerBeforeFailedMove = subject.ActivePlayer;

                var actualMoveInTheSameCorner = subject.Move(BoardPosition.TopLeft);
                actualMoveInTheSameCorner.Should().BeFalse();
                activePlayerBeforeFailedMove.Should().Be(subject.ActivePlayer);
            }
        }

        [TestFixture]
        public class WithAnyHorizontalWinner : AndMakingMovesOnTheBoard
        {
            [Test]
            public void ShouldRaiseGameFinishedEventWhenTopHorizontalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheTopRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.TopHorizontal);
            }

            [Test]
            public void ShouldRaiseGameFinishedEventWhenMiddleHorizontalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheMiddleRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.MiddleHorizontal);
            }

            [Test]
            public void ShouldRaiseGameFinishedEventWhenBottomHorizontalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheBottomRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.BottomHorizontal);
            }

            private void MakeTheBottomRowOfZerosTheWinner(IBoard subject)
            {
                string expectedBottomRowWinnerBoard =
                    "- | - | -".NewRow() +
                    "X | X | -".NewRow() +
                    "0 | 0 | 0".NewRow();
                subject.Move(BoardPosition.BottomLeft);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.BottomMiddle);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.BottomRight);
                expectedBottomRowWinnerBoard.Should().Be(subject.ToString());
            }

            private void MakeTheMiddleRowOfZerosTheWinner(IBoard subject)
            {
                string expectedMiddleRowWinnerBoard =
                    "X | X | -".NewRow() +
                    "0 | 0 | 0".NewRow() +
                    "- | - | -".NewRow();
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.MiddleRight);
                expectedMiddleRowWinnerBoard.Should().Be(subject.ToString());
            }

            private static void MakeTheTopRowOfZerosTheWinner(IBoard subject)
            {
                string expectedTopRowWinnerBoard =
                    "0 | 0 | 0".NewRow() +
                    "X | X | -".NewRow() +
                    "- | - | -".NewRow();
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.TopRight);
                expectedTopRowWinnerBoard.Should().Be(subject.ToString());
            }
        }

        [TestFixture]
        public class WithAnyVerticalWinner : AndMakingMovesOnTheBoard
        {
            [Test]
            public void ShouldRaiseGameFinishedEventWhenLeftVerticalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheLeftVerticalRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.LeftVertical);
            }

            [Test]
            public void ShouldRaiseGameFinishedEventWhenTheMiddleVerticalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheMiddleVerticalRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.MiddleVertical);
            }

            [Test]
            public void ShouldRaiseGameFinishedEventWhenTheRightVerticalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheRightVerticalRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.RightVertical);
            }

            private void MakeTheRightVerticalRowOfZerosTheWinner(IBoard subject)
            {
                string expectedRightVerticalWinnerBoard =
                    "- | X | 0".NewRow() +
                    "- | X | 0".NewRow() +
                    "- | - | 0".NewRow();
                subject.Move(BoardPosition.TopRight);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.MiddleRight);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.BottomRight);
                expectedRightVerticalWinnerBoard.Should().Be(subject.ToString());
            }

            private void MakeTheMiddleVerticalRowOfZerosTheWinner(IBoard subject)
            {
                string expectedMiddleVerticalWinnerBoard =
                    "X | 0 | -".NewRow() +
                    "X | 0 | -".NewRow() +
                    "- | 0 | -".NewRow();
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.BottomMiddle);
                expectedMiddleVerticalWinnerBoard.Should().Be(subject.ToString());
            }

            private static void MakeTheLeftVerticalRowOfZerosTheWinner(IBoard subject)
            {
                string expectedRightVerticalWinnerBoard =
                    "0 | X | -".NewRow() +
                    "0 | X | -".NewRow() +
                    "0 | - | -".NewRow();
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.BottomLeft);
                expectedRightVerticalWinnerBoard.Should().Be(subject.ToString());
            }
        }

        [TestFixture]
        public class WithAnyDiagonalWinner : AndMakingMovesOnTheBoard
        {
            [Test]
            public void ShouldRaiseGameFinishedEventWhenLeftDiagonalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheLeftDiagonalRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.LeftDiagonal);
            }

            [Test]
            public void ShouldRaiseGameFinishedEventWhenTheRightDiagonalRowHasAllZeros()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                MakeTheRightDiagonalRowOfZerosTheWinner(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
                gameFinishedArgument.LineWin.Should().Be(LineWin.RightDiagonal);
            }

            private void MakeTheRightDiagonalRowOfZerosTheWinner(IBoard subject)
            {
                string expectedRightDiagonalWinnerBoard =
                    "X | - | 0".NewRow() +
                    "X | 0 | -".NewRow() +
                    "0 | - | -".NewRow();
                subject.Move(BoardPosition.TopRight);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.BottomLeft);
            }

            private static void MakeTheLeftDiagonalRowOfZerosTheWinner(IBoard subject)
            {
                string expectedLeftDiagonalWinnerBoard =
                    "0 | X | X".NewRow() +
                    "- | 0 | -".NewRow() +
                    "- | - | 0".NewRow();
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.TopRight);
                subject.Move(BoardPosition.BottomRight);
            }
        }

        [TestFixture]
        public class WithADraw : AndMakingMovesOnTheBoard
        {
            [Test]
            public void ShouldRaiseGameFinishedEventWithDrawAndNoWinner()
            {
                IBoard subject = new Board();
                GameFinishedArgs gameFinishedArgument = null;
                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                SetupTheBoardForADraw(subject);

                gameFinishedArgument.Result.Should().Be(GameResult.Draw);
                gameFinishedArgument.LineWin.Should().Be(LineWin.None);
            }

            private void SetupTheBoardForADraw(IBoard subject)
            {
                string expectedTieBoard =
                    "0 | X | 0".NewRow() +
                    "0 | 0 | X".NewRow() +
                    "X | 0 | X".NewRow();
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.TopRight);
                subject.Move(BoardPosition.BottomRight);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.MiddleRight);
                subject.Move(BoardPosition.BottomMiddle);
                subject.Move(BoardPosition.BottomLeft);
                subject.Move(BoardPosition.TopLeft);
                expectedTieBoard.Should().Be(subject.ToString());
            }
        }
    }
}
