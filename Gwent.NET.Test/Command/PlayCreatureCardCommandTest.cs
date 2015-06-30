using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Xunit;

namespace Gwent.NET.Test.Command
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

    }
}