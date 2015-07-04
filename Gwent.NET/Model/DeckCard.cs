using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gwent.NET.Model
{
    public class DeckCard
    {
        [Key]
        public long Id { get; set; }

        //public long DeckId { get; set; }

        //public long CardId { get; set; }

        //[ForeignKey("DeckId")]
        [InverseProperty("Cards")]
        public virtual Deck Deck { get; set; }

        //[ForeignKey("CardId")]
        [InverseProperty("DeckCards")]
        public virtual Card Card { get; set; }
    }
}
