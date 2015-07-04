using System.ComponentModel.DataAnnotations;

namespace Gwent.NET.Model.States.Substates
{
    public abstract class Substate
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
