using System.Collections.Generic;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.DTOs
{
    public class PlayerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsLobbyOwner { get; set; }

        public bool IsPassing { get; set; }
        public bool IsTurn { get; set; }
        public int Lives { get; set; }

        public GwintFaction Faction { get; set; }
        public long? BattleKingCard { get; set; }
        public bool CanUseBattleKingCard { get; set; }
        
        public int HandCardCount { get; set; }
        public int DeckCardCount { get; set; }

        public List<long> HandCards { get; set; }
        public List<long> GraveyardCards { get; set; }
        public List<long> MeleeCards { get; set; }
        public List<long> RangeCards { get; set; }
        public List<long> SiegeCards { get; set; }
        public List<long> WeatherCards { get; set; }
        public List<long> MeleeModifiers { get; set; }
        public List<long> RangedModifiers { get; set; }
        public List<long> SiegeModifiers { get; set; }
    }
}