using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Events;
using Gwent.NET.Extensions;
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

        public long SenderUserId { get; set; }
        public ICollection<Event> Events { get; set; }
        public State NextState { get; set; }

        public abstract void Execute(Game game);
        
        public void TransitionToNextState(Game game)
        {
            if (NextState == null)
            {
                return;
            }

            var nextStateEvents = NextState.Initialize(game);
            Events.AddRange(nextStateEvents);
            game.State = NextState;

            var stateChangeEvents = game.Players.Select(player => new StateChangeEvent(player.User.Id)
            {
                Game = game.ToPersonalizedDto(player.User.Id)
            });

            Events.AddRange(stateChangeEvents);
        }
    }
}
