using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gwent.NET.Model
{
    public class PlayerCardSlot
    {
        [Key, Column( Order = 0)]
        public int PlayerId { get; set; }

        [Key, Column(Order = 1)]
        public int CardId { get; set; }

        public GwintSlot Slot { get; set; }

        public int EffectivePower { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [InverseProperty("CardSlots")]
        public virtual Player Player { get; set; }

        [InverseProperty("PlayerSlots")]
        public virtual Card Card { get; set; }

    }
}