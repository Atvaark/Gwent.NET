using System;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class StartGameCommand : Command
    {
        public override void Execute(Game game)
        {
            LobbyState state = game.State as LobbyState;
            if (state == null)
            {
                throw new CommandException();
            }
            Player sender = game.GetPlayerByUserId(SenderUserId);
            if (sender == null)
            {
                throw new CommandException();
            }
            if (!sender.IsOwner)
            {
                throw new CommandException();
            }
            if (game.Players.Count != Constants.MinPlayerCount)
            {
                throw new CommandException();
            }

            foreach (var player in game.Players)
            {
                player.DeckCards.AddRange(player.Deck.Cards);
                player.BattleKingCard = player.Deck.BattleKingCard; // TODO: Trigger battle king card abilities such as "Cancel leader cards"
                player.Faction = player.Deck.Faction;
            }

            State nextState;
            if (game.Players.Any(p => p.Deck.Faction == GwintFaction.Scoiatael))
            {
                nextState = new PickStartingPlayerState();
            }
            else
            {
                var startingPlayer = game.Players.OrderBy(p => new Guid()).First();
                startingPlayer.IsRoundStarter = true;
                Events.Add(new CoinTossEvent(game.GetAllUserIds())
                {
                    StartingPlayerId = startingPlayer.User.Id
                });
                nextState = new RedrawState();
            }
            
            SetNextState(game, nextState);
        }
    }
}