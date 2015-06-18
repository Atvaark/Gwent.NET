using System.Collections.Generic;

namespace Gwent.NET.Events
{
    public class CoinTossEvent : Event
    {
        public int StartingPlayerId { get; set; }

        public CoinTossEvent(IEnumerable<int> eventRecipients)
            : base(eventRecipients)
        {
        }

    }
}
