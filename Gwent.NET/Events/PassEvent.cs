using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class PassEvent : Event
    {
        public PassEvent(IEnumerable<long> eventRecipients)
            : base(eventRecipients)
        {
        }
    }
}