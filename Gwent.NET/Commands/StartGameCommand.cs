using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class StartGameCommand : Command
    {
        private const int RequiredPlayerCount = 2;

        public override IEnumerable<Event> Execute(Game game)
        {
            State nextState;
            if (game.Players.Any(p => p.Deck.Faction == GwentFaction.Scoiatael))
            {
                nextState = new PickStartingPlayerState();
            }
            else
            {
                // TODO: Chose a starting player ramdomly
                nextState = new RedrawState();
            }
            
            return SetNextState(game, nextState);
        }

        public override void Validate(Game game)
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
        }
    }
}