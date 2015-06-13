using System;
using System.Linq;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class StartGameCommand : Command
    {
        private const int RequiredPlayerCount = 2;

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
            if (game.Players.Count != RequiredPlayerCount)
            {
                throw new CommandException();
            }

            State nextState;
            if (game.Players.Any(p => p.Deck.Faction == GwentFaction.Scoiatael))
            {
                nextState = new PickStartingPlayerState();
            }
            else
            {
                var startingPlayer = game.Players.OrderBy(p => new Guid()).First();
                startingPlayer.IsRoundStarter = true;
                nextState = new RedrawState();
            }
            
            SetNextState(game, nextState);
        }
    }
}