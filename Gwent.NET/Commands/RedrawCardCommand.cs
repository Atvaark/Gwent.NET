using System;
using System.Collections.Generic;
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

        public override IEnumerable<Event> Execute(Game game)
        {
            RedrawState state = game.State as RedrawState;
            Player sender = game.GetPlayerByUserId(SenderUserId);
            Player opponent = game.GetOpponentPlayerByUserId(SenderUserId);
            var substate = state.Substates.FirstOrDefault(s => s.UserId == SenderUserId);
            var opponentSubstate = state.Substates.FirstOrDefault(s => s.UserId == opponent.User.Id);
            substate.RedrawCardCount -= 1;

            var card = sender.HandCards.First(c => c.Id == CardId);
            sender.HandCards.Remove(card);
            sender.DeckCards.Add(card);

            var deckCards = sender.DeckCards.ToList();
            deckCards.Shuffle(new Random());
            var drawnCard = deckCards.First();
            deckCards.Remove(drawnCard);
            sender.HandCards.Add(drawnCard);
            sender.DeckCards = deckCards;

            if (substate.RedrawCardCount != 0 || opponentSubstate.RedrawCardCount != 0)
            {
                yield break;
            }

            var nextState = new RoundState();
            foreach (var changeStateEvent in SetNextState(game, nextState))
            {
                yield return changeStateEvent;
            }
        }

        public override void Validate(Game game)
        {
            RedrawState state = game.State as RedrawState;
            if (state == null)
            {
                throw new CommandException();
            }
            Player sender = game.GetPlayerByUserId(SenderUserId);
            if (sender == null)
            {
                throw new CommandException();
            }
            var substate = state.Substates.FirstOrDefault(s => s.UserId == sender.User.Id);
            if (substate == null || substate.RedrawCardCount == 0)
            {
                throw new CommandException();
            }

            if (sender.HandCards.All(c => c.Id != CardId))
            {
                throw new CommandException();
            }
        }
    }
}