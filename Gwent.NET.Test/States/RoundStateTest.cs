using System.Linq;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.States
{
    public class RoundStateTest
    {
        [Fact]
        public void InitializeRoundState()
        {
            var state = new RoundState();
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                IsRoundStarter = true
            };
            var player2 = new Player
            {
                User = new User
                {
                    Id = 2
                },
                IsRoundStarter = false
            };
            var game = new Game
            {
                Players =
                {
                    player1,
                    player2
                },
                State = state
            };


            var events = state.Initialize(game).ToList();

            Assert.Equal(2, events.Count);
            Assert.True(player1.IsTurn);
            Assert.False(player2.IsTurn);
        }

        [Fact]
        public void InitializeRoundStateWhileOtherPlayerIsRoundStarter()
        {
            var state = new RoundState();
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                IsRoundStarter = false
            };
            var player2 = new Player
            {
                User = new User
                {
                    Id = 2
                },
                IsRoundStarter = true
            };
            var game = new Game
            {
                Players =
                {
                    player1,
                    player2
                },
                State = state
            };


            var events = state.Initialize(game).ToList();

            Assert.Equal(2, events.Count);
            Assert.False(player1.IsTurn);
            Assert.True(player2.IsTurn);
        }

    }
}