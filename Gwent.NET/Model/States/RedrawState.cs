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
        private const int InitialBuffedHandCardCount = 11;

        public List<RedrawPlayerSubstate> Substates { get; set; }

        public RedrawState()
        {
            Substates = new List<RedrawPlayerSubstate>();
        }
        
        public override IEnumerable<Event> Initialize(Game game)
        {
            foreach (var player in game.Players)
            {
                yield return DrawInitialCards(player);
                Substates.Add(new RedrawPlayerSubstate
                {
                    UserId = player.User.Id,
                    RedrawCardCount = InitialRedrawCount
                });
            }
        }
        
        private HandChangedEvent DrawInitialCards(Player player)
        {
            Random random = new Random();
            var cards = player.Deck.Cards.ToList();
            cards.Shuffle(random);
            var handCardCount = InitialHandCardCount;
            if (player.Deck.BattleKingCard.GetGwintEffects().HasFlag(GwintEffect.EleventhCard))
            {
                handCardCount = InitialBuffedHandCardCount;
            }

            var handCards = cards.Take(handCardCount).ToList();
            cards.RemoveRange(0, handCardCount);

            foreach (var card in cards)
            {
                player.DeckCards.Add(card);
            }
            foreach (var handCard in handCards)
            {
                player.HandCards.Add(handCard);
            }

            return new HandChangedEvent(new[] { player.User.Id })
            {
                HandCards = handCards.Select(c => c.Id).ToList()
            };
        }
    }
}
