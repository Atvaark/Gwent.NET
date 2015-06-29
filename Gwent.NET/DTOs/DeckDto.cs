using System.Collections.Generic;
using Gwent.NET.Model;

namespace Gwent.NET.DTOs
{
    public class DeckDto
    {
        public int Id { get; set; }
        public List<int> Cards { get; set; }
        public GwintFaction Faction { get; set; }
        public int BattleKingCard { get; set; }
        public bool IsPrimaryDeck { get; set; }
    }
}