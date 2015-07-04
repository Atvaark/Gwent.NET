using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class CoinTossEvent : Event
    {
        public long StartingPlayerId { get; set; }

        public CoinTossEvent(IEnumerable<long> eventRecipients)
            : base(eventRecipients)
        {
        }

    }
}
