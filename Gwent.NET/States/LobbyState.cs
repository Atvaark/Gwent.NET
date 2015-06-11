using System;
using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;

namespace Gwent.NET.States
{
    public class LobbyState : State
    {
        public override bool IsJoinable
        {
            get { return true; }
        }

        public override IEnumerable<Event> Initialize(Game game)
        {
            throw new NotImplementedException();
        }
    }
}