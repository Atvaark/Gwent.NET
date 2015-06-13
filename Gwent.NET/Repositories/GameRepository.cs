using System;
using System.Collections.Generic;
using System.Linq;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IGwintContext _context;

        public GameRepository(IGwintContext gwintContext)
        {
            _context = gwintContext;
        }
        
        public IEnumerable<Game> Get()
        {
            return _context.Games;
        }

        public Game Find(int id)
        {
            return _context.Games.Find(id);
        }

        public IEnumerable<Game> FindByUserId(int userId)
        {
            return _context.Games.Where(g => g.Players.Any(p => p.User.Id == userId));
        }

        public Game Create(Game game)
        {
            var newGame = _context.Games.Create();
            newGame.State = game.State;
            newGame.Players = game.Players;
            _context.Games.Add(newGame);
            _context.SaveChanges();
            return newGame;
        }

        public void Update(int id, Game game)
        {
            var existingGame = _context.Games.Find(id);
            if (existingGame == null)
            {
                throw new ArgumentException("id");
            }
            existingGame.Players.Clear();
            existingGame.State = game.State;
            _context.SaveChanges();
        }
    }
}