using System.Collections.Generic;

namespace Gwent.NET.Model
{
    public class Player
    {
        public Player()
        {
            HandCards = new List<Card>();
            DeckCards = new List<Card>();
            DisposedCards = new List<Card>();
            CloseCombatCards = new List<Card>();
            RangeCards = new List<Card>();
            SiegeCards = new List<Card>();
        }

        public User User { get; set; }
        public Deck Deck { get; set; }
        public bool IsOwner { get; set; }
        public int Lives { get; set; }
        public List<Card> HandCards { get; set; }
        public List<Card> DeckCards { get; set; }
        public List<Card> DisposedCards { get; set; }
        public List<Card> CloseCombatCards { get; set; }
        public List<Card> RangeCards { get; set; }
        public List<Card> SiegeCards { get; set; }
    }
}