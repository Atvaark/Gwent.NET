using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class PlayGlobalCardCommandTest
    {

        [Fact]
        public void PlayScorchWithSingleCreature()
        {
            var scorchCard = new Card
            {
                Id = 1,
                Types = GwintType.GlobalEffect,
                Effect = GwintEffect.Scorch
            };
            var creatureCard = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Melee
            };
            var player1HandCards = new List<PlayerCard>
            {
                scorchCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = creatureCard
                }
            };
            var player1GraveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee
            };

            command.Execute(game);
            

            Assert.Empty(player1CardSlots);
            var expectedGraveyardCards = new List<Card>()
            {
                creatureCard,
                scorchCard
            };
            Assert.Equal(expectedGraveyardCards, player1GraveyardCards.Select(g => g.Card));
        }

        [Fact]
        public void PlayScorchWithTwoIdenticalPowerCreatures()
        {
            var scorchCard = new Card
            {
                Id = 1,
                Types = GwintType.GlobalEffect,
                Effect = GwintEffect.Scorch
            };
            var creatureCard1 = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Melee
            };
            var creatureCard2 = new Card
            {
                Id = 3,
                Types = GwintType.Creature | GwintType.Melee
            };
            var player1HandCards = new List<PlayerCard>
            {
                scorchCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = creatureCard1,
                    EffectivePower = 1
                },
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = creatureCard2,
                    EffectivePower = 1
                }
            };
            var player1GraveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee
            };

            command.Execute(game);
            
            Assert.Empty(player1CardSlots);
            var expectedGraveyardCards = new List<Card>()
            {
                creatureCard1,
                creatureCard2,
                scorchCard
            };
            Assert.Equal(expectedGraveyardCards, player1GraveyardCards.Select(g => g.Card));
        }

        [Fact]
        public void PlayScorchWithTwoDifferentPowerCreatures()
        {
            var scorchCard = new Card
            {
                Id = 1,
                Types = GwintType.GlobalEffect,
                Effect = GwintEffect.Scorch
            };
            var creatureCard1 = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Melee
            };
            var creatureCard2 = new Card
            {
                Id = 3,
                Types = GwintType.Creature | GwintType.Melee
            };
            var player1HandCards = new List<PlayerCard>
            {
                scorchCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = creatureCard1,
                    EffectivePower = 1
                },
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = creatureCard2,
                    EffectivePower = 2
                }
            };
            var player1GraveyardCards = new List<PlayerCard>();
            var game = TestGameProvider.CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee
            };

            command.Execute(game);


            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Melee, cardSlot.Slot);
            Assert.Equal(creatureCard1, cardSlot.Card);
            var expectedGraveyardCards = new List<Card>()
            {
                creatureCard2,
                scorchCard
            };
            Assert.Equal(expectedGraveyardCards, player1GraveyardCards.Select(g => g.Card));
        }

    }
}