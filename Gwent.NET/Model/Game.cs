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
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public virtual State State { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        
        public Player GetPlayerByUserId(int userId)
        {
            return Players.FirstOrDefault(p => p.User.Id == userId);
        }

        public Player GetOpponentPlayerByUserId(int userId)
        {
            return Players.FirstOrDefault(p => p.User.Id != userId);
        }

        public IEnumerable<int> GetAllUserIds()
        {
            return Players.Select(p => p.User.Id);
        }
    }
}