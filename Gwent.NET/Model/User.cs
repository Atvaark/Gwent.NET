using System.Collections.Generic;

namespace Gwent.NET.Model
{
    public class User
    {
        public User()
        {
            Decks = new List<Deck>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public List<Deck> Decks { get; set; }
    }
}