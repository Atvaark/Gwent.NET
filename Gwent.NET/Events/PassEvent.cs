using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class PassEvent : Event
    {
        public PassEvent(IEnumerable<int> eventRecipients)
            : base(eventRecipients)
        {
        }
    }
}