using System.ComponentModel.DataAnnotations;

namespace Gwent.NET.Model.States.Substates
{
    public abstract class Substate
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
