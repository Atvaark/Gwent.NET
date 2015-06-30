using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Model;
using Xunit;

namespace Gwent.NET.Test.Command
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
            var player1HandCards = new List<Card>
            {
                card
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots);
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
            var player1HandCards = new List<Card>
            {
                card
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots);
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
            var player1HandCards = new List<Card>
            {
                card
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots);
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
            var player1HandCards = new List<Card>
            {
                clearSkyCard
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
            var graveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
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
                clearSkyCard
            };

            Assert.Equal(expectedGraveyardCards, graveyardCards, EqualityComparer<Card>.Default);
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
            var player1HandCards = new List<Card>
            {
                card1
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = card2
                }
            };
            var graveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
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
            Assert.Equal(card2, graveyardCard);
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
            var player1HandCards = new List<Card>
            {
                card1
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = card2
                }
            };
            var graveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
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
            Assert.Equal(card2, graveyardCard);
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
            var player1HandCards = new List<Card>
            {
                card1
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Weather,
                    Card = card2
                }
            };
            var graveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots, graveyardCards);
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
            Assert.Equal(card2, graveyardCard);
        }


    }
}