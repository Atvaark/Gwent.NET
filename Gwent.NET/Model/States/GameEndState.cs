using System;
using System.Collections.Generic;
using Gwent.NET.Events;

namespace Gwent.NET.Model.States
{
    public class GameEndState : State
    {
        public override bool IsOver
        {
            get { return true; }
        }

        public override IEnumerable<Event> Initialize(Game game)
        {
            throw new NotImplementedException();
        }
    }
}