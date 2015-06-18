using System.Collections.Generic;
using Gwent.NET.Events;

namespace Gwent.NET.Model.States
{
    public class LobbyState : State
    {
        public override string Name
        {
            get { return "Lobby"; }
        }

        public override IEnumerable<Event> Initialize(Game game)
        {


            yield break;
        }
    }
}