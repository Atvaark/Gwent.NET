using System;
using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Model.States.Substates;

namespace Gwent.NET.Model.States
{
    public class RedrawState : State
    {
        private const int InitialRedrawCount = 2;
        private const int InitialHandCardCount = 10;
        private const int InitialIncreasedHandCardCount = 11;

        public virtual ICollection<RedrawPlayerSubstate> Substates { get; set; }


        public RedrawState()
        {
            Substates = new HashSet<RedrawPlayerSubstate>();
        }
        
        public override IEnumerable<Event> Initialize(Game game)
        {
            return game.Players.Select(InitializeHand);
        }

        private HandChangedEvent InitializeHand(Player player)
        {
            var initialHandCardCount = GetInitialHandCardCount(player);
            var shuffledCards = GetShuffledCards(player);

            var handCards = shuffledCards.Take(initialHandCardCount).ToList();
            shuffledCards.RemoveRange(0, initialHandCardCount);
            foreach (var handCard in handCards)
            {
                player.HandCards.Add(handCard);
            }

            foreach (var card in shuffledCards)
            {
                player.DeckCards.Add(card);
            }
            
            Substates.Add(new RedrawPlayerSubstate
            {
                UserId = player.User.Id,
                RedrawCardCount = InitialRedrawCount
            });

            return new HandChangedEvent(new[] { player.User.Id })
            {
                HandCards = handCards.Select(c => c.Id).ToList()
            };
        }

        private static List<Card> GetShuffledCards(Player player)
        {
            var shuffledCards = player.Deck.Cards.ToList();
            shuffledCards.Shuffle();
            return shuffledCards;
        }

        private static int GetInitialHandCardCount(Player player)
        {
            var handCardCount = InitialHandCardCount;
            if (player.Deck.BattleKingCard.Effects.HasFlag(GwintEffect.EleventhCard))
            {
                handCardCount = InitialIncreasedHandCardCount;
            }

            return handCardCount;
        }
    }
}
