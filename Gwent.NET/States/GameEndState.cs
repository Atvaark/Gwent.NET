using System;
using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;

namespace Gwent.NET.States
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