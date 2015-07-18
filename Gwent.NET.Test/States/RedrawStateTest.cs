using System;
using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.States
{
    public class RedrawStateTest
    {
        [Fact]
        public void InitializeRedrawState()
        {
            long nextCardId = 1;
            var createNewCards = new Func<int, ICollection<PlayerCard>>(count =>
            {
                var cards = new List<PlayerCard>();
                for (int i = 0; i < count; i++)
                {
                    cards.Add(new PlayerCard
                    {
                        Id = nextCardId++,
                        Card = new Card()
                    });
                }
                return cards;
            });
            
            var state = new RedrawState();
            var player = new Player
            {
                User = new User
                {
                    Id = 1
                },
                DeckCards = createNewCards(Constants.MinDeckCardCount).ToList(),
                BattleKingCard = createNewCards(1).Single()
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
            long nextCardId = 1;
            var createNewCards = new Func<int, ICollection<PlayerCard>>(count =>
            {
                var cards = new List<PlayerCard>();
                for (int i = 0; i < count; i++)
                {
                    cards.Add(new PlayerCard
                    {
                        Id = nextCardId++,
                        Card = new Card()
                    });
                }
                return cards;
            });

            var player1BattleKingCard = createNewCards(1).Single();
            player1BattleKingCard.Card.Effect = GwintEffect.EleventhCard;

            var player = new Player
            {
                User = new User
                {
                    Id = 1
                },
                DeckCards = createNewCards(Constants.MinDeckCardCount).ToList(),
                BattleKingCard = player1BattleKingCard,
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