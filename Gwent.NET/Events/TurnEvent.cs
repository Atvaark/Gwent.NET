using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class TurnEvent : Event
    {
        public int TurnUserId { get; set; }

        public TurnEvent(IEnumerable<int> eventRecipients) : base(eventRecipients)
        {
        }
    }
}
