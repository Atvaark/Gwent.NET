using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gwent.NET.Events;
using Newtonsoft.Json;

namespace Gwent.NET.Model.States
{
    public abstract class State
    {
        [Key]
        public int Id { get; set; }

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