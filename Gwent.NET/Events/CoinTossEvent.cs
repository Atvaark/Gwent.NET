using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class CoinTossEvent : Event
    {
        public CoinTossEvent(IEnumerable<int> eventRecipients) : base(eventRecipients)
        {
        }
    }
}
