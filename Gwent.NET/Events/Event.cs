using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Gwent.NET.Events
{
    public class Event
    {
        private readonly List<long> _recipients;

        public string Name
        {
            get { return GetType().Name; }
        }

        [JsonIgnore]
        public IEnumerable<long> Recipients
        {
            get { return _recipients; }
        }

        public Event(long eventRecipient)
        {
            _recipients = new List<long> { eventRecipient };
        }

        public Event(IEnumerable<long> eventRecipients)
        {
            _recipients = eventRecipients.ToList();
        }
    }
}