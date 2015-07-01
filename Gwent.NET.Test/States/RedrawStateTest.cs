using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.States
{
    public class RedrawStateTest
    {
        [Fact]
        public void InitializeRedrawState()
        {
            var state = new RedrawState();
            var player = new Player
            {
                User = new User
                {
                    Id = 1
                },
                DeckCards = Enumerable.Repeat(new Card(), Constants.MinDeckCardCount).ToList(),
                BattleKingCard = new Card()
            };
            var game = new Game
            {
                Players =
                {
                    player
                },
                State = state
            };

            var events = state.Initialize(game).ToList();

            var handChangedEvent = events.SingleOrDefault();
            Assert.NotNull(handChangedEvent);
            Assert.IsType(typeof(HandChangedEvent), handChangedEvent);
            Assert.Equal(Constants.InitialHandCardCount, player.HandCards.Count);
            Assert.True(player.IsTurn);
            Assert.Equal(Constants.MinDeckCardCount - Constants.InitialHandCardCount, player.DeckCards.Count);
            Assert.Contains(state.Substates, s => s.UserId == 1 && s.RedrawCardCount == Constants.InitialRedrawCount);
        }
        
        [Fact]
        public void InitializeRedrawStateWithEleventhCardEffect()
        {
            var player = new Player
            {
                User = new User
                {
                    Id = 1
                },
                DeckCards = Enumerable.Repeat(new Card(), Constants.MinDeckCardCount).ToList(),
                BattleKingCard = new Card
                {
                    Effect = GwintEffect.EleventhCard
                },
                CanUseBattleKingCard = true
            };
            var game = new Game
            {
                Players =
                {
                    player
                }
            };
            var state = new RedrawState();

            var events = state.Initialize(game).ToList();

            var handChangedEvent = events.SingleOrDefault();
            Assert.NotNull(handChangedEvent);
            Assert.IsType(typeof(HandChangedEvent), handChangedEvent);
            Assert.Equal(Constants.InitialIncreasedHandCardCount, player.HandCards.Count);
            Assert.Equal(Constants.MinDeckCardCount - Constants.InitialIncreasedHandCardCount, player.DeckCards.Count);
        }


    }
}