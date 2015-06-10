using System.Collections.Generic;
using System.Linq;

namespace Gwent.NET.Events
{
    public class Event
    {
        private readonly List<int> _recipients;

        public Event(IEnumerable<int> eventRecipients)
        {
            _recipients = eventRecipients.ToList();
        }

        public IEnumerable<int> Recipients
        {
            get { return _recipients; }
        }
    }
}