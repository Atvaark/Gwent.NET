using System;
using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;

namespace Gwent.NET.Commands
{
    public class EndRedrawCardCommand : Command
    {
        public override IEnumerable<Event> Execute(int senderUserId, Game game)
        {
            throw new NotImplementedException();
        }
    }
}
