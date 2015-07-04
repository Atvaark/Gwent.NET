using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gwent.NET.Model
{
    public class User
    {
        public User()
        {
            Decks = new HashSet<Deck>();
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string Picture { get; set; }

        public virtual ICollection<Deck> Decks { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}