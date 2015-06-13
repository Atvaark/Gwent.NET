using System.Linq;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Model.States.Substates;

namespace Gwent.NET.Commands
{
    public class EndRedrawCardCommand : Command
    {
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

            RedrawPlayerSubstate substate = state.Substates.FirstOrDefault(s => s.UserId == sender.User.Id);
            RedrawPlayerSubstate opponentSubstate = state.Substates.FirstOrDefault(s => s.UserId == opponent.User.Id);
            if (substate == null || opponentSubstate == null)
            {
                throw new CommandException();
            }

            substate.RedrawCardCount = 0;
            if (opponentSubstate.RedrawCardCount != 0)
            {
                return;
            }

            var nextState = new RoundState();
            SetNextState(game, nextState);
        }
    }
}
