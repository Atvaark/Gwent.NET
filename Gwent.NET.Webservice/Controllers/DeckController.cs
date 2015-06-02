using System.Linq;
using System.Web.Http;
using Gwent.NET.DTOs;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Webservice.Controllers
{
    [Authorize]
    public class DeckController : AuthenticatedApiController
    {
        private readonly ICardRepository _cardRepository;

        public DeckController(IUserRepository userRepository, ICardRepository cardRepository) : base(userRepository)
        {
            _cardRepository = cardRepository;
        }

        // TODO: Remove {userId} in the routes and use the claims instead.

        // GET: api/deck
        [Route("api/deck")]
        public IHttpActionResult Get()
        {
            User user;
            if(!TryGetUser(out user))
            {
                return BadRequest();
            }
            return Ok(user.Decks.Select(d => d.ToDto()));
        }

        // GET: api/deck/5
        [Route("api/deck/{deckId}")]
        public IHttpActionResult Get(int deckId)
        {
            User user;
            if (!TryGetUser(out user))
            {
                return BadRequest();
            }
            Deck deck = user.Decks.ElementAtOrDefault(deckId);
            if (deck == null)
            {
                return NotFound();
            }
            return Ok(deck.ToDto());
        }

        // POST: api/Deck
        [Route("api/deck")]
        public IHttpActionResult Post([FromBody]DeckDto deck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user;
            if (!TryGetUser(out user))
            {
                return BadRequest();
            }
            var newDeck = DeckDtoToDeck(deck);
            if (!ValidateDeck(newDeck))
            {
                return BadRequest();
            }
            UserRepository.AddDeck(user.Id, newDeck);
            return Ok(newDeck.ToDto());
        }
        
        // PUT: api/Deck/5
        [Route("api/deck/{deckId}")]
        public IHttpActionResult Put(int userId, int deckId, [FromBody]DeckDto deck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = UserRepository.FindById(userId);
            if (user == null)
            {
                return NotFound();
            }
            Deck existingDeck = user.Decks.ElementAtOrDefault(deckId);
            if (existingDeck == null)
            {
                return NotFound();
            }
            var newDeck = DeckDtoToDeck(deck);
            if (!ValidateDeck(newDeck))
            {
                return BadRequest();
            }
            existingDeck.BattleKingCard = newDeck.BattleKingCard;
            existingDeck.Cards = newDeck.Cards;
            existingDeck.Faction = newDeck.Faction;
            return Ok();
        }

        private Deck DeckDtoToDeck(DeckDto deck)
        {
            Deck newDeck = new Deck
            {
                BattleKingCard = _cardRepository.Find(deck.BattleKingCard),
                Faction = deck.Faction,
                Cards = deck.Cards.Select(id => _cardRepository.Find(id)).ToList()
            };
            return newDeck;
        }

        private bool ValidateDeck(Deck deck)
        {
            // Enough cards in the deck
            if (deck.Cards.Count < 22)
            {
                return false;
            }

            // None of the cards is a battle king
            if (deck.Cards.Any(c => c.IsBattleKing))
            {
                return false;
            }
            
            // The battle king card is actually a battle king
            if (!deck.BattleKingCard.IsBattleKing)
            {
                return false;
            }
            
            // The faction of the battle king card is not neutral
            if (deck.BattleKingCard.FactionIndex == GwentFaction.Neutral)
            {
                return false;
            }
            
            // Not too many special cards
            int specialCardCount = deck.Cards.Count(c => IsSpecialCard(c.GetGwintType()));
            if (specialCardCount > 10)
            {
                return false;
            }

            // All cards are either neutral or belong to the battle king faction
            var battleKingFaction = deck.BattleKingCard.FactionIndex;
            bool allCardsNeutralOrBattleKingFaction = deck.Cards
                .All(c => c.FactionIndex == battleKingFaction || c.FactionIndex == GwentFaction.Neutral);
            if (!allCardsNeutralOrBattleKingFaction)
            {
                return false;
            }

            return true;
        }

        private bool IsSpecialCard(GwintType gwintType)
        {
            return gwintType.HasFlag(GwintType.Spell)
                   || gwintType.HasFlag(GwintType.RowModifier)
                   || gwintType.HasFlag(GwintType.GlobalEffect) 
                   || gwintType.HasFlag(GwintType.Weather);
        }
    }
}
