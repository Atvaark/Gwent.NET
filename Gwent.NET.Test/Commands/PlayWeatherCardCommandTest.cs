using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Extensions;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class PlayWeatherCardCommandTest
    {
        [Fact]
        public void PlayMeleeWeatherCard()
        {
            var card = new Card
            {
                Id = 1,
                Types = GwintType.Weather,
                Effect = GwintEffect.Melee
            };
            var player1HandCards = new List<PlayerCard>
            {
                card.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Weather
            };
    
            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Weather, cardSlot.Slot);
            Assert.Equal(card, cardSlot.Card);
        }
        
        [Fact]
        public void PlayRangedWeatherCard()
        {
            var card = new Card
            {
                Id = 1,
                Types = GwintType.Weather,
                Effect = GwintEffect.Ranged
            };
            var player1HandCards = new List<PlayerCard>
            {
                card.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Weather
            };
    
            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Weather, cardSlot.Slot);
            Assert.Equal(card, cardSlot.Card);
        }
        
        [Fact]
        public void PlaySiegeWeatherCard()
        {
            var card = new Card
            {
                Id = 1,
                Types = GwintType.Weather,
                Effect = GwintEffect.Siege
            };
            var player1HandCards = new List<PlayerCard>
            {
                card.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Weather
            };
    
            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Weather, cardSlot.Slot);
            Assert.Equal(card, cardSlot.Card);
        }
        
        [Fact]
        public void PlayClearSkyWeatherCard()
        {
            var clearSkyCard = new Card
            {
                Id = 1,
                Types = GwintType.Weather,
                Effect = GwintEffect.ClearSky
            };
            var weatherCard1 = new Card
            {
                Id = 2,
                Types = GwintType.Weather,
                Effect = GwintEffect.Melee
            };
            var weatherCard2 = new Card
            {
                Id = 3,
                Types = GwintType.Weather,
                Effect = GwintEffect.Ranged
            };
            var weatherCard3 = new Card
            {
                Id = 4,
                Types = GwintType.Weather,
                Effect = GwintEffect.Siege
            };
            var player1HandCards = new List<PlayerCard>
            {
                clearSkyCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = weatherCard1
                },
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = weatherCard2
                },
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = weatherCard3
                },
            };
            var graveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Weather
            };
    
            command.Execute(game);

            Assert.Empty(player1CardSlots);

            var expectedGraveyardCards = new List<Card>()
            {
                weatherCard1,
                weatherCard2,
                weatherCard3,
                clearSkyCard,
            };

            Assert.Equal(expectedGraveyardCards, graveyardCards.Select(g => g.Card), EqualityComparer<Card>.Default);
        }
        
        [Fact]
        public void PlayAndReplaceMeleeWeatherCard()
        {
            var card1 = new Card
            {
                Id = 1,
                Types = GwintType.Weather,
                Effect = GwintEffect.Melee
            };
            var card2 = new Card
            {
                Id = 2,
                Types = GwintType.Weather,
                Effect = GwintEffect.Melee
            };
            var player1HandCards = new List<PlayerCard>
            {
                card1.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = card2
                }
            };
            var graveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Weather
            };
    
            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Weather, cardSlot.Slot);
            Assert.Equal(card1, cardSlot.Card);

            var graveyardCard = graveyardCards.Single();
            Assert.Equal(card2, graveyardCard.Card);
        }
        
        [Fact]
        public void PlayAndReplaceRangedWeatherCard()
        {
            var card1 = new Card
            {
                Id = 1,
                Types = GwintType.Weather,
                Effect = GwintEffect.Ranged
            };
            var card2 = new Card
            {
                Id = 2,
                Types = GwintType.Weather,
                Effect = GwintEffect.Ranged
            };
            var player1HandCards = new List<PlayerCard>
            {
                card1.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = card2
                }
            };
            var graveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Weather
            };
    
            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Weather, cardSlot.Slot);
            Assert.Equal(card1, cardSlot.Card);
            
            var graveyardCard = graveyardCards.Single();
            Assert.Equal(card2, graveyardCard.Card);
        }
        
        [Fact]
        public void PlayAndReplaceSiegeWeatherCard()
        {
            var card1 = new Card
            {
                Id = 1,
                Types = GwintType.Weather,
                Effect = GwintEffect.Siege
            };
            var card2 = new Card
            {
                Id = 2,
                Types = GwintType.Weather,
                Effect = GwintEffect.Siege
            };
            var player1HandCards = new List<PlayerCard>
            {
                card1.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = card2
                }
            };
            var graveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Weather
            };
    
            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Weather, cardSlot.Slot);
            Assert.Equal(card1, cardSlot.Card);
            
            var graveyardCard = graveyardCards.Single();
            Assert.Equal(card2, graveyardCard.Card);
        }


    }
}