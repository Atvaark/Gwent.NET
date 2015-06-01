using System.Linq;
using System.Web.Http;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.Repositories;

namespace Gwent.NET.Webservice.Controllers
{
    [Authorize]
    public class CardController : ApiController
    {
        private readonly ICardRepository _cardRepository;

        public CardController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public CardController()
        {
            _cardRepository = new CardRepository();
        }
        
        // GET: api/Card
        public IHttpActionResult Get()
        {
            return Ok(_cardRepository.Get().Select(c => c.ToDto()));
        }

        // GET: api/Card/5
        public IHttpActionResult Get(int id)
        {
            Card card = _cardRepository.Find(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card.ToDto());
        }
    }
}
