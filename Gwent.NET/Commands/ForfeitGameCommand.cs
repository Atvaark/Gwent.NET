using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class ForfeitGameCommand : Command
    {
        public override void Execute(Game game)
        {
            Player sender = game.GetPlayerByUserId(SenderUserId);
            Player opponent = game.GetOpponentPlayerByUserId(SenderUserId);
            if (sender == null)
            {
                throw new CommandException();
            }
            sender.IsTurn = false;

            if (opponent != null)
            {
                opponent.IsTurn = false;
                opponent.IsVictor = true;
            }

            var forfeitEvent = new ForfeitEvent(game.GetAllUserIds())
            {
                UserId = sender.User.Id
            };
            Events.Add(forfeitEvent);

            var nextState = new GameEndState();
            SetNextState(game, nextState);
        }
    }
}