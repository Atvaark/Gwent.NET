using System.Collections.Generic;

namespace Gwent.NET.DTOs
{
    public class PlayerDto
    {
        public int User { get; set; }
        public bool IsLobbyOwner { get; set; }
        public int Lives { get; set; }
        public int HandCardCount { get; set; }
        public int DeckCardCount { get; set; }
        public List<int> HandCards { get; set; }
        public List<int> GraveyardCards { get; set; }
        public List<int> MeleeCards { get; set; }
        public List<int> RangeCards { get; set; }
        public List<int> SiegeCards { get; set; }
        public List<int> WeatherCards { get; set; }
        public List<int> MeleeModifiers { get; set; }
        public List<int> RangedModifiers { get; set; }
        public List<int> SiegeModifiers { get; set; }
    }
}