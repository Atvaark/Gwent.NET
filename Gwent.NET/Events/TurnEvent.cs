using System.Collections.Generic;
using Gwent.NET.DTOs;

namespace Gwent.NET.Events
{
    public class TurnEvent : Event
    {
        public GameDto Game { get; set; }
        public TurnEvent(int eventRecipient)
            : base(eventRecipient)
        {

        }
    }
}
