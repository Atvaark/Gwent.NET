using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gwent.NET.Model
{
    public class Deck
    {
        public Deck()
        {
            Cards = new HashSet<Card>();
        }

        [Key]
        public int Id { get; set; }

        public bool IsPrimaryDeck { get; set; }

        public GwintFaction Faction { get; set; }

        public virtual Card BattleKingCard { get; set; }

        public virtual ICollection<Card> Cards { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}