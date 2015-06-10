using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;

namespace Gwent.NET.Commands
{
    public class PickStartingPlayerCommand : Command
    {
        public int StartPlayerId { get; set; }
        public override IEnumerable<Event> Execute(int senderUserId, Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}
