using System.Collections.Generic;
using Gwent.NET.Events;

namespace Gwent.NET.Model.States
{
    public class GameEndState : State
    {
        public override string Name
        {
            get { return "GameEnd"; }
        }

        public override IEnumerable<Event> Initialize(Game game)
        {
            game.IsActive = false;
            yield break;
        }
    }
}