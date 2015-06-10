using System;
using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.States;

namespace Gwent.NET.Commands
{
    public class StartGameCommand : Command
    {
        private const int InitialHandCardCount = 10;
        private const int RequiredPlayerCount = 2;

        public override IEnumerable<Event> Execute(int senderUserId, Game game)
        {
            Validate(senderUserId, game);
            
            Player senderPlayer = game.GetPlayerByUserId(senderUserId);
            Player opponentPlayer = game.GetOpponentPlayerByUserId(senderUserId);

            yield return DrawInitialCards(senderPlayer);
            yield return DrawInitialCards(opponentPlayer);
            game.State = new RedrawState();
            yield return new StateChangeEvent(game.GetAllUserIds())
            {
                State = game.State
            };
        }

        private HandChangedEvent DrawInitialCards(Player player)
        {
            Random random = new Random();
            var cards = player.Deck.Cards.ToList();
            cards.Shuffle(random);
            var handCards = cards.Take(InitialHandCardCount).ToList();
            player.DeckCards.AddRange(cards);
            player.HandCards.AddRange(handCards);
            return new HandChangedEvent(new[] {player.User.Id})
            {
                HandCards = handCards.Select(c => c.Index).ToList()
            };

        }

        private static void Validate(int senderUserId, Game game)
        {
            LobbyState state = game.State as LobbyState;
            if (state == null)
            {
                throw new InvalidCommandException();
            }
            Player senderPlayer = game.GetPlayerByUserId(senderUserId);
            if (senderPlayer == null)
            {
                throw new InvalidCommandException();
            }
            if (!senderPlayer.IsOwner)
            {
                throw new InvalidCommandException();
            }
            if (game.Players.Count != RequiredPlayerCount)
            {
                throw new InvalidCommandException();
            }
        }
    }
}