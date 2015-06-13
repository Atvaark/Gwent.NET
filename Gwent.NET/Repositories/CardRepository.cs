using System.Collections.Generic;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly IGwintContext _context;

        public CardRepository(IGwintContext context)
        {
            _context = context;
        }

        public IEnumerable<Card> Get()
        {
            return _context.Cards;
        }

        public Card Find(int id)
        {
            return _context.Cards.Find(id);
        }
    }
}