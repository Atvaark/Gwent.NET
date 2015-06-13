using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public GwentFaction Faction { get; set; }

        [InverseProperty("BattleKingCardDecks")]
        public virtual Card BattleKingCard { get; set; }

        public virtual ICollection<Card> Cards { get; set; }
    }
}