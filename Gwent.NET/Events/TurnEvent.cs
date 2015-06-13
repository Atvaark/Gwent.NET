using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class TurnEvent : Event
    {
        public TurnEvent(IEnumerable<int> eventRecipients) : base(eventRecipients)
        {
        }
    }
}
