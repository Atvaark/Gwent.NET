using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class ForfeitEvent : Event
    {
        public long UserId { get; set; }

        public ForfeitEvent(IEnumerable<long> eventRecipients)
            : base(eventRecipients)
        {
        }
    }
}