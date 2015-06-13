using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public class ForfeitGameCommand : Command
    {
        public override IEnumerable<Event> Execute(Game game)
        {
            Player sender = game.GetPlayerByUserId(SenderUserId);
            yield return new ForfeitEvent(game.GetAllUserIds())
            {
                UserId = sender.User.Id
            };
            var nextState = new GameEndState();
            foreach (var changeStateEvent in SetNextState(game, nextState))
            {
                yield return changeStateEvent;
            }
        }

        public override void Validate(Game game)
        {
            Player sender = game.GetPlayerByUserId(SenderUserId);
            if (sender == null)
            {
                throw new CommandException();
            }
        }
    }
}