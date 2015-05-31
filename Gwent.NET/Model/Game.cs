using System.Collections.Generic;
using Gwent.NET.States;

namespace Gwent.NET.Model
{
    public class Game
    {
        public Game()
        {
            Players = new List<Player>();
        }
        public int Id { get; set; }
        public State State { get; set; }
        public List<Player> Players { get; set; } 
    }
}