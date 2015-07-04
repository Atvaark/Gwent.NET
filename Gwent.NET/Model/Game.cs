using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Gwent.NET.Model.States;

namespace Gwent.NET.Model
{
    public class Game
    {
        public Game()
        {
            Players = new HashSet<Player>();
        }

        [Key]
        public long Id { get; set; }

        public bool IsActive { get; set; }

        public virtual State State { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Player GetPlayerByUserId(long userId)
        {
            return Players.FirstOrDefault(p => p.User.Id == userId);
        }

        public Player GetOpponentPlayerByUserId(long userId)
        {
            return Players.FirstOrDefault(p => p.User.Id != userId);
        }

        public IEnumerable<long> GetAllUserIds()
        {
            return Players.Select(p => p.User.Id);
        }
    }
}