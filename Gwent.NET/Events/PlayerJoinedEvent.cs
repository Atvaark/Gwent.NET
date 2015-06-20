using Gwent.NET.DTOs;

namespace Gwent.NET.Events
{
    public class PlayerJoinedEvent : Event
    {
        public GameDto Game { get; set; }

        public PlayerJoinedEvent(int eventRecipient)
            : base(eventRecipient)
        {

        }
    }
}
