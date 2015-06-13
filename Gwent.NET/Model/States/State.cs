using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gwent.NET.Events;

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
        
        public abstract IEnumerable<Event> Initialize(Game game);
    }
}