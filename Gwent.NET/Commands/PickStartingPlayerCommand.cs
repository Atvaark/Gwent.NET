using System;
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

        public override void Execute(Game game)
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

            var substate = state.Substates.First(s => s.UserId == SenderUserId);
            if (substate == null || !substate.CanPickStartingPlayer || substate.StartingPlayerId.HasValue)
            {
                throw new CommandException();
            }

            if (!game.GetAllUserIds().Contains(StartingPlayerId))
            {
                throw new CommandException();
            }

            substate.StartingPlayerId = StartingPlayerId;
            sender.IsTurn = false;

            if (state.Substates.Any(s => s.CanPickStartingPlayer && !s.StartingPlayerId.HasValue))
            {
                return;
            }

            DetermineStartingPlayer(game, state);
            var nextState = new RedrawState();
            SetNextState(game, nextState);
        }

        private void DetermineStartingPlayer(Game game, PickStartingPlayerState state)
        {
            if (state.Substates.All(s => !s.CanPickStartingPlayer || s.StartingPlayerId == StartingPlayerId))
            {
                var startingPlayer = game.GetOpponentPlayerByUserId(StartingPlayerId);
                startingPlayer.IsRoundStarter = true;
            }
            else
            {
                var startingPlayer = game.Players.OrderBy(p => new Guid()).First();
                startingPlayer.IsRoundStarter = true;
                Events.Add(new CoinTossEvent(game.GetAllUserIds()));
            }
        }
    }
}
