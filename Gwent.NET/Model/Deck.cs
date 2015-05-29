using System.Collections.Generic;

namespace Gwent.NET.Model
{
    public class Deck
    {
        public List<DeckCard> Cards { get; set; }
        public GwentFaction Faction { get; set; }
        public DeckCard BattleKingCard { get; set; }
    }
}