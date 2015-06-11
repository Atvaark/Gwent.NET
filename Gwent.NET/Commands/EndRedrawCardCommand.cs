using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.States;

namespace Gwent.NET.Commands
{
    public class EndRedrawCardCommand : Command
    {
        public override IEnumerable<Event> Execute(Game game)
        {
            Player sender = game.GetPlayerByUserId(SenderUserId);
            Player opponent = game.GetOpponentPlayerByUserId(SenderUserId);
            
            RedrawState state = game.State as RedrawState;
            var substate = state.Substates.FirstOrDefault(s => s.UserId == sender.User.Id);
            var opponentSubstate = state.Substates.FirstOrDefault(s => s.UserId == opponent.User.Id);

            substate.RedrawCardCount = 0;
            if (opponentSubstate.RedrawCardCount != 0)
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
            if (substate == null)
            {
                throw new CommandException();
            }
        }
    }
}
