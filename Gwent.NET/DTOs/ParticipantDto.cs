using System.Collections.Generic;

namespace Gwent.NET.DTOs
{
    public class ParticipantDto
    {
        public int User { get; set; }
        public int Deck { get; set; }
        public bool IsOwner { get; set; }
        public int Lives { get; set; }
        public int Draws { get; set; }
        public int DeckCardCount { get; set; }
        public List<int> HandCards { get; set; }
        public List<int> DisposedCards { get; set; }
        public List<int> CloseCombatCards { get; set; }
        public List<int> RangeCards { get; set; }
        public List<int> SiegeCards { get; set; }
    }
}