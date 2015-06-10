using System.Collections.Generic;
using Gwent.NET.States;

namespace Gwent.NET.Events
{
    public class StateChangeEvent : Event
    {
        public State State { get; set; }

        public StateChangeEvent(IEnumerable<int> eventRecipients) : base(eventRecipients)
        {

        }
    }
}
