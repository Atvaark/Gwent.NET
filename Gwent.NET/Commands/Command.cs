using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Commands
{
    public abstract class Command
    {
        public Command()
        {
            Events = new List<Event>();
        }

        public int SenderUserId { get; set; }
        public ICollection<Event> Events { get; set; }

        public abstract void Execute(Game game);
        
        protected void SetNextState(Game game, State nextState)
        {
            var nextStateEvents = nextState.Initialize(game);
            Events.AddRange(nextStateEvents);
            game.State = nextState;
            var stateChangeEvent = new StateChangeEvent(game.GetAllUserIds())
            {
                State = nextState
            };
            Events.Add(stateChangeEvent);
        }
    }
}
