using System.Collections.Generic;
using Gwent.NET.Model;

namespace Gwent.NET.Interfaces
{
    public interface ICardRepository
    {
        IEnumerable<Card> Get();
        Card Find(int id);
    }
}