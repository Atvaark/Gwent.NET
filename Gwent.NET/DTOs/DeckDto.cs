using System.Collections.Generic;
using Gwent.NET.Model;

namespace Gwent.NET.DTOs
{
    public class DeckDto
    {
        public List<int> Cards { get; set; }
        public GwentFaction Faction { get; set; }
        public int BattleKingCard { get; set; }
    }
}