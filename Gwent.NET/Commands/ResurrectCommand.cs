﻿using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;

namespace Gwent.NET.Commands
{
    public class ResurrectCommand : Command
    {
        public int CardId { get; set; }
        public override IEnumerable<Event> Execute(int senderUserId, Game game)
        {
            throw new System.NotImplementedException();
        }
    }
}