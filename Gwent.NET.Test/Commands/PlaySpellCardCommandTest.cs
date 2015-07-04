using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Commands;
using Gwent.NET.Extensions;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Xunit;

namespace Gwent.NET.Test.Commands
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
            var player1HandCards = new List<PlayerCard>
            {
                unsummonDummyCard.ToPlayerCard()
            };
            var player1CardSlots = new List<PlayerCardSlot>
            {
                new PlayerCardSlot
                {
                    Slot = GwintSlot.Melee,
                    Card = targetCard
                }
            };
            var game = TestGameProvider.CreateGame(player1HandCards, player1CardSlots);
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
            Assert.Equal(targetCard, handCard.Card);
        }
    }
}
