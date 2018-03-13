using FluentAssertions;
using NUnit.Framework;

namespace Codurance.Domain.Unit.Tests
{
    [TestFixture]
    public class WhenUsingAPlayerToPlayTicTacToe
    {
        [Test]
        public void ShouldBeEqualWhenTheyHaveTheSameValues()
        {
            Player playerOne = new Player(Team.Cross, true, "Test Player");
            Player playerTwo = new Player(playerOne.Team, playerOne.IsActive, playerOne.Name);
            bool actual = playerOne.Equals(playerTwo);            
            actual.Should().BeTrue();
        }

        [Test]
        public void ShouldNotBeEqualWhenTheTeamIsDifferent()
        {
            Player playerOne = new Player(Team.Cross, true, "Test Player");
            Player playerTwo = new Player(Team.None, playerOne.IsActive, playerOne.Name);
            bool actual = playerOne.Equals(playerTwo);
            actual.Should().BeFalse();
        }

        [Test]
        public void ShouldNotBeEqualWhenTheIsActiveIsDifferent()
        {
            Player playerOne = new Player(Team.Cross, true, "Test Player");
            Player playerTwo = new Player(playerOne.Team, !playerOne.IsActive, playerOne.Name);
            bool actual = playerOne.Equals(playerTwo);
            actual.Should().BeFalse();
        }

        [Test]
        public void ShouldNotBeEqualWhenTheNameIsDifferent()
        {
            Player playerOne = new Player(Team.Cross, true, "Test Player");
            Player playerTwo = new Player(playerOne.Team, playerOne.IsActive, "Test Player 2");
            bool actual = playerOne.Equals(playerTwo);
            actual.Should().BeFalse();
        }

        [Test]
        public void ShouldNotBeEqualToNull()
        {
            Player playerOne = new Player(Team.Cross, true, "Test Player");
            bool actual = playerOne.Equals(null);
            actual.Should().BeFalse();
        }
    }
}

