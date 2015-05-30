using System.Collections.Generic;
using Gwent.NET.Model;

namespace Gwent.NET.Interfaces
{
    public interface IGameRepository
    {
        IEnumerable<Game> Get();
        Game Find(int id);
        Game Create(Game game);
        void Update(int id, Game game);
    }
}