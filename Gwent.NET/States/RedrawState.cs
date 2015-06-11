using System;
using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Gwent.NET.States.Substates;

namespace Gwent.NET.States
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
            if (player.Deck.BattleKingCard.GetGwintEffect().HasFlag(GwintEffect.EleventhCard))
            {
                handCardCount = InitialBuffedHandCardCount;
            }

            // TODO: remove the taken cards
            var handCards = cards.Take(handCardCount).ToList();
            cards.RemoveRange(0, handCardCount);
            player.DeckCards.AddRange(cards);
            player.HandCards.AddRange(handCards);
            return new HandChangedEvent(new[] { player.User.Id })
            {
                HandCards = handCards.Select(c => c.Index).ToList()
            };
        }
    }
}
