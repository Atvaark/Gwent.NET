using System;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class RedrawCardCommand : Command
    {
        public int CardId { get; set; }

        public override void Execute(Game game)
        {
            RedrawState state = game.State as RedrawState;
            if (state == null)
            {
                throw new CommandException();
            }

            Player sender = game.GetPlayerByUserId(SenderUserId);
            Player opponent = game.GetOpponentPlayerByUserId(SenderUserId);
            if (sender == null || opponent == null)
            {
                throw new CommandException();
            }

            var substate = state.Substates.FirstOrDefault(s => s.UserId == sender.User.Id);
            var opponentSubstate = state.Substates.FirstOrDefault(s => s.UserId == opponent.User.Id);
            if (substate == null || opponentSubstate == null || substate.RedrawCardCount == 0)
            {
                throw new CommandException();
            }

            if (sender.HandCards.All(c => c.Id != CardId))
            {
                throw new CommandException();
            }

            RedrawCard(sender);
            substate.RedrawCardCount -= 1;
            sender.IsTurn = substate.RedrawCardCount > 0;

            Events.Add(new HandChangedEvent(sender.User.Id)
            {
                HandCards = sender.HandCards.Select(c => c.Id).ToList()
            });
            
            if (substate.RedrawCardCount != 0 || opponentSubstate.RedrawCardCount != 0)
            {
                return;
            }

            var nextState = new RoundState();
            SetNextState(game, nextState);
        }

        private void RedrawCard(Player player)
        {
            var card = player.HandCards.First(c => c.Id == CardId);
            player.HandCards.Remove(card);
            player.DeckCards.Add(card);

            var deckCards = player.DeckCards.ToList();
            deckCards.Shuffle();
            var drawnCard = deckCards.First();
            deckCards.Remove(drawnCard);
            player.HandCards.Add(drawnCard);

            player.DeckCards.Clear();
            player.DeckCards.AddRange(deckCards);
        }
    }
}