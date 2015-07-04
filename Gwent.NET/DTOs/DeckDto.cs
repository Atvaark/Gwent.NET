using System.Collections.Generic;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.DTOs
{
    public class DeckDto
    {
        public long Id { get; set; }
        public List<long> Cards { get; set; }
        public GwintFaction Faction { get; set; }
        public long BattleKingCard { get; set; }
        public bool IsPrimaryDeck { get; set; }
    }
}