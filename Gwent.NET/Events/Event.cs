using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Gwent.NET.Events
{
    public class Event
    {
        private readonly List<int> _recipients;

        public string Name
        {
            get { return GetType().Name; }
        }

        [JsonIgnore]
        public IEnumerable<int> Recipients
        {
            get { return _recipients; }
        }

        public Event(int eventRecipient)
        {
            _recipients = new List<int> { eventRecipient };
        }

        public Event(IEnumerable<int> eventRecipients)
        {
            _recipients = eventRecipients.ToList();
        }
    }
}