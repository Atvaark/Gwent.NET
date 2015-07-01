using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Model.States.Substates;

namespace Gwent.NET.Model.States
{
    public class RedrawState : State
    {
        public virtual ICollection<RedrawPlayerSubstate> Substates { get; set; }

        public override string Name
        {
            get { return "Redraw"; }
        }

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
            player.IsTurn = true;
            var initialHandCardCount = GetInitialHandCardCount(player);
            var shuffledCards = GetShuffledDeckCards(player);

            var handCards = shuffledCards.Take(initialHandCardCount).ToList();
            shuffledCards.RemoveRange(0, initialHandCardCount);
            foreach (var handCard in handCards)
            {
                player.HandCards.Add(handCard);
            }

            player.DeckCards.Clear();
            foreach (var card in shuffledCards)
            {
                player.DeckCards.Add(card);
            }

            Substates.Add(new RedrawPlayerSubstate
            {
                UserId = player.User.Id,
                RedrawCardCount = Constants.InitialRedrawCount
            });

            return new HandChangedEvent(player.User.Id)
            {
                HandCards = handCards.Select(c => c.Id).ToList()
            };
        }

        private static List<Card> GetShuffledDeckCards(Player player)
        {
            var shuffledCards = player.DeckCards.ToList();
            shuffledCards.Shuffle();
            return shuffledCards;
        }

        private static int GetInitialHandCardCount(Player player)
        {
            var handCardCount = Constants.InitialHandCardCount;
            if (player.CanUseBattleKingCard && player.BattleKingCard.Effect.HasFlag(GwintEffect.EleventhCard))
            {
                handCardCount = Constants.InitialIncreasedHandCardCount;
            }

            return handCardCount;
        }
    }
}
