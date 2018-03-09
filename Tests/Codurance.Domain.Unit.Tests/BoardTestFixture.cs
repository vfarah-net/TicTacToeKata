using System;
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
            public void ShouldSwapPlayerForEverySuccessFullMove()
            {
                IBoard subject = new Board();
                var activePlayer = subject.ActivePlayer;

                var actual = subject.Move(BoardPosition.TopLeft);

                actual.Should().BeTrue();
                activePlayer.Should().NotBe(subject.ActivePlayer);
            }

            [Test]
            public void ShouldRaiseAPlayerSwappedEventForSuccessFullMove()
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
            }

            private void MakeTheBottomRowOfZerosTheWinner(IBoard subject)
            {
                subject.Move(BoardPosition.BottomLeft);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.BottomMiddle);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.BottomRight);
            }

            private void MakeTheMiddleRowOfZerosTheWinner(IBoard subject)
            {
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.MiddleRight);
            }

            private static void MakeTheTopRowOfZerosTheWinner(IBoard subject)
            {
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.TopRight);
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
            }

            private void MakeTheRightVerticalRowOfZerosTheWinner(IBoard subject)
            {
                subject.Move(BoardPosition.TopRight);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.MiddleRight);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.BottomRight);
            }

            private void MakeTheMiddleVerticalRowOfZerosTheWinner(IBoard subject)
            {
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.BottomMiddle);
            }

            private static void MakeTheLeftVerticalRowOfZerosTheWinner(IBoard subject)
            {
                subject.Move(BoardPosition.TopLeft);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.TopMiddle);
                subject.Move(BoardPosition.BottomLeft);
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
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

                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Win);
                gameFinishedArgument.Winner.Should().NotBeNull();
                gameFinishedArgument.Winner.Team.Should().Be(Team.Zero);
            }

            private void MakeTheRightDiagonalRowOfZerosTheWinner(IBoard subject)
            {
                subject.Move(BoardPosition.BottomLeft);
                subject.Move(BoardPosition.MiddleLeft);
                subject.Move(BoardPosition.BottomMiddle);
                subject.Move(BoardPosition.Middle);
                subject.Move(BoardPosition.BottomRight);
            }

            private static void MakeTheLeftDiagonalRowOfZerosTheWinner(IBoard subject)
            {
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
                string expectedTieBoard = 
                    "0 | X | 0" + Environment.NewLine +
                    "0 | 0 | X" + Environment.NewLine +
                    "X | 0 | X" + Environment.NewLine;

                subject.GameFinished += (GameFinishedArgs argument) =>
                {
                    gameFinishedArgument = argument;
                };

                SetupTheBoardForADraw(subject);

                expectedTieBoard.Should().Be(subject.ToString());
                gameFinishedArgument.Should().NotBeNull();
                gameFinishedArgument.Result.Should().Be(GameResult.Draw);
                gameFinishedArgument.Winner.Should().BeNull();                
            }

            private void SetupTheBoardForADraw(IBoard subject)
            {
                // Set the board to visually look like :
                //  0 | X | 0
                //  0 | 0 | X
                //  X | 0 | X
                subject.Move(BoardPosition.Middle);         // 0
                subject.Move(BoardPosition.TopMiddle);      // X 
                subject.Move(BoardPosition.TopRight);       // 0 
                subject.Move(BoardPosition.BottomRight);    // X
                subject.Move(BoardPosition.MiddleLeft);     // 0
                subject.Move(BoardPosition.MiddleRight);    // X
                subject.Move(BoardPosition.BottomMiddle);   // 0
                subject.Move(BoardPosition.BottomLeft);     // X
                subject.Move(BoardPosition.TopLeft);        // 0
            }
        }
    }
}
