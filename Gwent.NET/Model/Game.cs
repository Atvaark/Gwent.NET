using System.Collections.Generic;
using Gwent.NET.States;

namespace Gwent.NET.Model
{
    public class Game
    {
        public int Id { get; set; }
        public State State { get; set; }
        public List<Participant> Participants { get; set; } 
    }
}