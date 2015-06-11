using System.Collections.Generic;
using Gwent.NET.Events;
using Gwent.NET.Model;
using Newtonsoft.Json;

namespace Gwent.NET.States
{
    public abstract class State
    {

        public string Name
        {
            get { return GetType().Name; }
        }

        [JsonIgnore]
        public virtual bool IsOver
        {
            get { return false; }
        }

        [JsonIgnore]
        public virtual bool IsJoinable
        {
            get { return false; }
        }
        
        public abstract IEnumerable<Event> Initialize(Game game);
    }
}