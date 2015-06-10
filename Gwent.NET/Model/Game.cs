using System.Collections.Generic;
using System.Linq;
using Gwent.NET.States;

namespace Gwent.NET.Model
{
    public class Game
    {
        public Game()
        {
            Players = new List<Player>();
        }

        public int Id { get; set; }
        public State State { get; set; }
        public List<Player> Players { get; set; }

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