using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class HandChangedEvent : Event
    {
        public List<long> HandCards { get; set; }

        public HandChangedEvent(long eventRecipient)
            : base(eventRecipient)
        {

        }
    }
}
