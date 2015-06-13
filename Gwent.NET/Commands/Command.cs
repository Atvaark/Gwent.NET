using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public abstract class Command
    {
        public int SenderUserId { get; set; }

        public abstract IEnumerable<Event> Execute(Game game);

        // TODO: Move the validation logic into excecute when done implementing all commands.
        public abstract void Validate(Game game);

        protected IEnumerable<Event> SetNextState(Game game, State nextState)
        {
            foreach (var initializationEvent in nextState.Initialize(game))
            {
                yield return initializationEvent;
            }
            game.State = nextState;
            yield return new StateChangeEvent(game.GetAllUserIds())
            {
                State = nextState
            };
        }
    }
}
