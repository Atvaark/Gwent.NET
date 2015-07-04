using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.States
{
    public class PickStartingPlayerStateTest
    {
        [Fact]
        public void InitializePickStartingPlayerWithOneScoiataelDeck()
        {
            var state = new PickStartingPlayerState();
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                Deck = new Deck
                {
                    Faction = GwintFaction.Scoiatael
                }
            };
            var player2 = new Player
            {
                User = new User
                {
                    Id = 2
                },
                Deck = new Deck()
            };
            var game = new Game
            {
                Players =
                {
                    player1,
                    player2,
                },
                State = state
            };

            var events = state.Initialize(game).ToList();
            
            Assert.Empty(events);
            Assert.True(player1.IsTurn);
            Assert.False(player2.IsTurn);
            Assert.Contains(state.Substates, s => s.UserId == 1 && s.StartingPlayerUserId == null && s.CanPickStartingPlayer);
            Assert.Contains(state.Substates, s => s.UserId == 2 && s.StartingPlayerUserId == null && !s.CanPickStartingPlayer);
        }

        [Fact]
        public void InitializePickStartingPlayerWithTwoScoiataelDecks()
        {
            var player1 = new Player
            {
                User = new User
                {
                    Id = 1
                },
                Deck = new Deck
                {
                    Faction = GwintFaction.Scoiatael
                }
            };
            var player2 = new Player
            {
                User = new User
                {
                    Id = 2
                },
                Deck = new Deck
                {
                    Faction = GwintFaction.Scoiatael
                }
            };
            var game = new Game
            {
                Players = new List<Player>
                {
                    player1,
                    player2,
                }
            };
            var state = new PickStartingPlayerState();

            var events = state.Initialize(game).ToList();
            
            Assert.Empty(events);
            Assert.True(player1.IsTurn);
            Assert.True(player2.IsTurn);
            Assert.Contains(state.Substates, s => s.UserId == 1 && s.StartingPlayerUserId == null && s.CanPickStartingPlayer);
            Assert.Contains(state.Substates, s => s.UserId == 2 && s.StartingPlayerUserId == null && s.CanPickStartingPlayer);
        }

    }
}
