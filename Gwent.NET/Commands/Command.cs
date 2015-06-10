using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;

namespace Gwent.NET.Commands
{
    public abstract class Command
    {
        public abstract IEnumerable<Event> Execute(int senderUserId, Game game);
    }
}
