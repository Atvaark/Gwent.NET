using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class PlayerJoinedEvent : Event
    {
        public PlayerJoinedEvent(IEnumerable<int> eventRecipients) : base(eventRecipients)
        {
        }
    }
}
