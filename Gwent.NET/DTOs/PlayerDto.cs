using System.Collections.Generic;

namespace Gwent.NET.DTOs
{
    public class PlayerDto
    {
        public int User { get; set; }
        public int Deck { get; set; }
        public bool IsLobbyOwner { get; set; }
        public int Lives { get; set; }
        public int HandCardCount { get; set; }
        public int DeckCardCount { get; set; }
        public List<int> DisposedCards { get; set; }
        public List<int> CloseCombatCards { get; set; }
        public List<int> RangeCards { get; set; }
        public List<int> SiegeCards { get; set; }
    }
}