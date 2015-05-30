using System;
using System.Collections.Generic;
using System.Threading;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.States;

namespace Gwent.NET.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly Dictionary<int, Game> _games;
        private int _id;

        public GameRepository()
        {
            _games = new Dictionary<int, Game>();
        }

        public IEnumerable<Game> Get()
        {
            return _games.Values;
        }

        public Game Find(int id)
        {
            Game game;
            _games.TryGetValue(id, out game);
            return game;
        }

        public Game Create(Game game)
        {
            Game newGame = new Game
            {
                Id = Interlocked.Increment(ref _id),
                State = game.State
            };
            _games.Add(newGame.Id, newGame);
            return newGame;
        }

        public void Update(int id, Game game)
        {
            if (!_games.ContainsKey(id) || game.Id != id)
            {
                throw new ArgumentException("id");
            }

            _games[id] = game;
        }
    }
}