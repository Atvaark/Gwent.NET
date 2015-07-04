using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Extensions;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class PlayRowModifierCardCommandTest
    {
        [Fact]
        public void PlayMeleeHorn()
        {
            var hornCard = new Card
            {
                Id = 1,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var player1HandCards = new List<PlayerCard>
            {
                hornCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.MeleeModifier
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.MeleeModifier, cardSlot.Slot);
            Assert.Equal(hornCard, cardSlot.Card);
        }


        [Fact]
        public void PlayRangedHorn()
        {
            var hornCard = new Card
            {
                Id = 1,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var player1HandCards = new List<PlayerCard>
            {
                hornCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.RangedModifier
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.RangedModifier, cardSlot.Slot);
            Assert.Equal(hornCard, cardSlot.Card);
        }


        [Fact]
        public void PlaySiegeHorn()
        {
            var hornCard = new Card
            {
                Id = 1,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var player1HandCards = new List<PlayerCard>
            {
                hornCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.SiegeModifier
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.SiegeModifier, cardSlot.Slot);
            Assert.Equal(hornCard, cardSlot.Card);
        }

        [Fact]
        public void PlayAndReplaceMeleeHorn()
        {
            var hornCard = new Card
            {
                Id = 1,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var existingHornCard = new Card
            {
                Id = 2,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var player1HandCards = new List<PlayerCard>
            {
                hornCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.MeleeModifier,
                    Card = existingHornCard
                }
            };
            var player1GraveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots, player1GraveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.MeleeModifier
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.MeleeModifier, cardSlot.Slot);
            Assert.Equal(hornCard, cardSlot.Card);

            var graveyardCard = player1GraveyardCards.Single();
            Assert.Equal(existingHornCard, graveyardCard.Card);
        }


        [Fact]
        public void PlayAndReplaceRangedHorn()
        {
            var hornCard = new Card
            {
                Id = 1,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var existingHornCard = new Card
            {
                Id = 2,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var player1HandCards = new List<PlayerCard>
            {
                hornCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.RangedModifier,
                    Card = existingHornCard
                }
            };
            var player1GraveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots, player1GraveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.RangedModifier
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.RangedModifier, cardSlot.Slot);
            Assert.Equal(hornCard, cardSlot.Card);
            
            var graveyardCard = player1GraveyardCards.Single();
            Assert.Equal(existingHornCard, graveyardCard.Card);
        }


        [Fact]
        public void PlayAndReplaceSiegeHorn()
        {
            var hornCard = new Card
            {
                Id = 1,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var existingHornCard = new Card
            {
                Id = 2,
                Types = GwintType.RowModifier | GwintType.Melee | GwintType.Ranged | GwintType.Siege,
                Effect = GwintEffect.Horn
            };
            var player1HandCards = new List<PlayerCard>
            {
                hornCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.SiegeModifier,
                    Card = existingHornCard
                }
            };
            var player1GraveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots, player1GraveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.SiegeModifier
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.SiegeModifier, cardSlot.Slot);
            Assert.Equal(hornCard, cardSlot.Card);

            var graveyardCard = player1GraveyardCards.Single();
            Assert.Equal(existingHornCard, graveyardCard.Card);
        }


    }
}