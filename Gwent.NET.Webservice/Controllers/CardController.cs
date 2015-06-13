using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using Gwent.NET.DTOs;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Webservice.Controllers
{
    [Authorize]
    public class CardController : ApiController
    {
        private readonly IGwintContext _context;

        public CardController(IGwintContext context)
        {
            _context = context;
        }

        // GET: api/Card
        public IHttpActionResult Get()
        {
            var cardDtos = _context.Cards
                .AsEnumerable()
                .Select(c => c.ToDto());
            return Ok(cardDtos);
        }

        // GET: api/Card/5
        public IHttpActionResult Get(int id)
        {
            Card card = _context.Cards.Find(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card.ToDto());
        }
    }
}
