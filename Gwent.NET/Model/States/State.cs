using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gwent.NET.Events;

namespace Gwent.NET.Model.States
{
    public abstract class State
    {
        [Key]
        public int Id { get; set; }
        
        public abstract string Name { get; }
        
        public abstract IEnumerable<Event> Initialize(Game game);
    }
}