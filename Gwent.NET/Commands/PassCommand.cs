using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class PassCommand : Command
    {
        public override void Execute(Game game)
        {
            RoundState state = game.State as RoundState;
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

            if (!sender.IsTurn)
            {
                throw new CommandException();
            }

            if (opponent.IsPassing)
            {
                // TODO: Calculate round winner, begin a new round or end the game.
            }


            sender.IsPassing = true;
            sender.IsTurn = false;
            opponent.IsTurn = true;

            Events.Add(new PassEvent(new[] { opponent.User.Id }));
        }
    }
}