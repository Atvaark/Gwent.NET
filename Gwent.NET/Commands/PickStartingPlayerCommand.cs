using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class PickStartingPlayerCommand : Command
    {
        public int StartingPlayerId { get; set; }

        public override IEnumerable<Event> Execute(Game game)
        {
            PickStartingPlayerState state = game.State as PickStartingPlayerState;
            var substate = state.Substates.First(s => s.UserId == SenderUserId);
            substate.StartingPlayerId = StartingPlayerId;
            if (state.Substates.Any(s => s.CanPickStartingPlayer && !s.StartingPlayerId.HasValue))
            {
                yield break;
            }

            // TODO: If both players picked each other as the starting player then flip a coin.

            var nextState = new RedrawState();
            foreach (var changeStateEvent in SetNextState(game, nextState))
            {
                yield return changeStateEvent;
            }
        }

        public override void Validate(Game game)
        {
            PickStartingPlayerState state = game.State as PickStartingPlayerState;
            if (state == null)
            {
                throw new CommandException();
            }
            Player sender = game.GetPlayerByUserId(SenderUserId);
            if (sender == null)
            {
                throw new CommandException();
            }

            var senderChoice = state.Substates.FirstOrDefault(s => s.UserId == SenderUserId);
            if (senderChoice == null || !senderChoice.CanPickStartingPlayer || senderChoice.StartingPlayerId.HasValue)
            {
                throw new CommandException();
            }
        }
    }
}
