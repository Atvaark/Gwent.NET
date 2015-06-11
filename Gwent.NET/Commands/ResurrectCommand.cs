using System;
using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;

namespace Gwent.NET.Commands
{
    public class ResurrectCommand : Command
    {
        public int CardId { get; set; }
        public override IEnumerable<Event> Execute(Game game)
        {
            throw new NotImplementedException();
        }

        public override void Validate(Game game)
        {
            throw new NotImplementedException();
        }
    }
}