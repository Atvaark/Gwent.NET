using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class ForfeitEvent : Event
    {
        public int UserId { get; set; }

        public ForfeitEvent(IEnumerable<int> eventRecipients) : base(eventRecipients)
        {
        }
    }
}