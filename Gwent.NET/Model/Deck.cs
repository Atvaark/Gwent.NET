using System.Collections.Generic;

namespace Gwent.NET.Model
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();
        }

        public int Id { get; set; }
        public List<Card> Cards { get; set; }
        public GwentFaction Faction { get; set; }
        public Card BattleKingCard { get; set; }
    }
}