using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Model;
using Xunit;

namespace Gwent.NET.Test.Command
{
    public class PlaySpellCardCommandTest
    {
        [Fact]
        public void PlayUnsummunDummySpellCard()
        {
            var unsummonDummyCard = new Card
            {
                Id = 1,
                Types = GwintType.Spell,
                Effect = GwintEffect.UnsummonDummy
            };
            var targetCard = new Card
            {
                Id = 2,
                Types = GwintType.Creature | GwintType.Melee
            };
            var player1HandCards = new List<Card>
            {
                unsummonDummyCard
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = targetCard
                }
            };
            var game = TestGameFactory.CreateGame(player1HandCards, player1CardSlots);
            var command = new PlayCardCommand
            {
                SenderUserId = 1,
                CardId = 1,
                Slot = GwintSlot.Melee,
                TargetCardId = 2
            };

            command.Execute(game);

            var cardSlot = player1CardSlots.Single();
            Assert.Equal(GwintSlot.Melee, cardSlot.Slot);
            Assert.Equal(unsummonDummyCard, cardSlot.Card);

            var handCard = player1HandCards.Single();
            Assert.Equal(targetCard, handCard);
        }
    }
}
