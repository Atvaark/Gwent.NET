using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gwent.NET.Model.Enums;

namespace Gwent.NET.Model
{
    public class PlayerCardSlot
    {
        [Key]
        public long Id { get; set; }

        public long PlayerId { get; set; }

        public long CardId { get; set; }

        public GwintSlot Slot { get; set; }

        public int EffectivePower { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("PlayerId")]
        [InverseProperty("CardSlots")]
        public virtual Player Player { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }

    }
}