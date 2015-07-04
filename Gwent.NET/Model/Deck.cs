using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.Model
{
    public class Deck
    {
        public Deck()
        {
            Cards = new HashSet<DeckCard>();
        }

        [Key]
        public long Id { get; set; }

        public bool IsPrimaryDeck { get; set; }

        public GwintFaction Faction { get; set; }

        public virtual DeckCard BattleKingCard { get; set; }
        
        public virtual ICollection<DeckCard> Cards { get; set; }
            
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}