using System.Collections.Generic;

namespace Gwent.NET.Model
{
    public class Participant
    {
        public Participant()
        {
            DeckCards = new List<DeckCard>();
            DisposedCards = new List<DeckCard>();
            CloseCombatCards = new List<DeckCard>();
            RangeCards = new List<DeckCard>();
            SiegeCards = new List<DeckCard>();
        }


        public User User { get; set; }
        public int Lives { get; set; }
        public Deck Deck { get; set; }
        public List<DeckCard> DeckCards { get; set; }
        public List<DeckCard> DisposedCards { get; set; }
        public List<DeckCard> CloseCombatCards { get; set; }
        public List<DeckCard> RangeCards { get; set; }
        public List<DeckCard> SiegeCards { get; set; }
    }
}