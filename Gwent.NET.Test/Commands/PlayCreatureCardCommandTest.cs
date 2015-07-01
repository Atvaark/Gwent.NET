using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Model;
using Xunit;

namespace Gwent.NET.Test.Commands
{
    public class PlayCreatureCardCommandTest
    {

        [Fact]
        public void PlayMeleeCreatureCard()
        {
            var card = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Melee
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
                Slot = GwintSlot.Melee
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Melee, cardSlot.Slot);
            Assert.Equal(card, cardSlot.Card);
        }

        [Fact]
        public void PlayRangedCreatureCard()
        {
            var card = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Ranged
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var player1HandCards = new List<Card>
            {
                card
            };
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Ranged
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Ranged, cardSlot.Slot);
            Assert.Equal(card, cardSlot.Card);
        }

        [Fact]
        public void PlaySiegeCreatureCard()
        {
            var card = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Siege
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
                Slot = GwintSlot.Siege
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Siege, cardSlot.Slot);
            Assert.Equal(card, cardSlot.Card);
        }

        [Fact]
        public void PlaySummonClonesCreatureCard()
        {
            var cloneCard = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Ranged
            };
            var summonClonesCard = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Melee,
                Effect = GwintEffect.SummonClones,
                SummonFlags = new List<GwintSummonFlag>
                {
                    new GwintSummonFlag
                    {
                        SummonCard = cloneCard
                    }
                }
            };
            var player1HandCards = new List<Card>
            {
                summonClonesCard,
                cloneCard
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var player1GraveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(
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

            Assert.Contains(player1CardSlots, s => s.Slot == GwintSlot.Melee && s.Card == summonClonesCard);
            Assert.Contains(player1CardSlots, s => s.Slot == GwintSlot.Ranged && s.Card == cloneCard);
        }

        [Fact]
        public void PlayNurseCreatureCard()
        {
            var nurseCreature = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Melee,
                Effect = GwintEffect.Nurse
            };
            var rangedCreature = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Ranged
            };
            var player1HandCards = new List<Card>
            {
                nurseCreature
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var player1GraveyardCards = new List<Card>
            {
                rangedCreature
            };
            var game = TestGameFactory.CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards);

            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee,
                TargetCardId = 2
            };

            command.Execute(game);

            Assert.Contains(player1CardSlots, s => s.Slot == GwintSlot.Melee && s.Card == nurseCreature);
            Assert.Contains(player1CardSlots, s => s.Slot == GwintSlot.Ranged && s.Card == rangedCreature);
        }

        [Fact]
        public void PlaySpyCreatureCard()
        {
            var spyCreature = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Melee | GwintType.Spy,
                Effect = GwintEffect.Draw2
            };
            var player1HandCards = new List<Card>
            {
                spyCreature
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var player1GraveyardCards = new List<Card>();
            var player2CardSlots = new List<PlayerCardSlot>();
            var player2GraveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards,
                player2CardSlots,
                player2GraveyardCards);

            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee
            };

            command.Execute(game);

            Assert.Empty(player1CardSlots);

            var player2CardSlot = player2CardSlots.Single();
            Assert.Equal(GwintSlot.Melee, player2CardSlot.Slot);
            Assert.Equal(spyCreature, player2CardSlot.Card);
        }

        [Fact]
        public void PlayMeleeScorchCreatureCardAboveThreshold()
        {
            var meleeScorchCreature = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Melee,
                Effect = GwintEffect.MeleeScorch
            };
            var meleeCreature = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Melee
            };
            var player1HandCards = new List<Card>
            {
                meleeScorchCreature
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var player1GraveyardCards = new List<Card>();
            var player2CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = meleeCreature,
                    EffectivePower = Constants.ScorchThresholdMelee
                }
            };
            var player2GraveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards,
                player2CardSlots,
                player2GraveyardCards);

            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee
            };

            command.Execute(game);

            var player1CardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Melee, player1CardSlot.Slot);
            Assert.Equal(meleeScorchCreature, player1CardSlot.Card);

            Assert.Empty(player2CardSlots);
            var player2GraveyardCard = player2GraveyardCards.Single();
            Assert.Equal(meleeCreature, player2GraveyardCard);
        }

        [Fact]
        public void PlayMeleeScorchCreatureCardBelowThreshold()
        {
            var meleeScorchCreature = new Card
            {
                Id = 1,
                Types = GwintType.Creature | GwintType.Melee,
                Effect = GwintEffect.MeleeScorch
            };
            var meleeCreature = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Melee
            };
            var player1HandCards = new List<Card>
            {
                meleeScorchCreature
            };
            var player1CardSlots = new List<PlayerCardSlot>();
            var player1GraveyardCards = new List<Card>();
            var player2CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = meleeCreature,
                    EffectivePower = Constants.ScorchThresholdMelee - 1
                }
            };
            var player2GraveyardCards = new List<Card>();
            var game = TestGameFactory.CreateGame(
                player1HandCards,
                player1CardSlots,
                player1GraveyardCards,
                player2CardSlots,
                player2GraveyardCards);

            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee
            };

            command.Execute(game);

            var player1CardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Melee, player1CardSlot.Slot);
            Assert.Equal(meleeScorchCreature, player1CardSlot.Card);

            var player2CardSlot = player2CardSlots.Single();
            Assert.Equal(GwintSlot.Melee, player2CardSlot.Slot);
            Assert.Equal(meleeCreature, player2CardSlot.Card);
        }

    }
}