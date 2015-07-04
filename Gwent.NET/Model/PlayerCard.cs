using System.ComponentModel.DataAnnotations;

namespace Gwent.NET.Model
{
    public class PlayerCard
    {
        [Key]
        public long Id { get; set; }
        
        public virtual Player Player { get; set; }

        public virtual Card Card { get; set; }
    }
}
