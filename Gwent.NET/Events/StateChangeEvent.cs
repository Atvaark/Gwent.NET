using Gwent.NET.DTOs;

namespace Gwent.NET.Events
{
    public class StateChangeEvent : Event
    {
        public GameDto Game { get; set; }

        public StateChangeEvent(int eventRecipient) : base(eventRecipient)
        {

        }
    }
}
