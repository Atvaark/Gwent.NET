using System.Linq;
using System.Web.Http;
using Gwent.NET.DTOs;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Webservice.Controllers
{
    [Authorize]
    public class DeckController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ICardRepository _cardRepository;

        public DeckController(IUserRepository userRepository, ICardRepository cardRepository)
        {
            _userRepository = userRepository;
            _cardRepository = cardRepository;
        }

        // GET: api/user/5/deck
        [Route("api/user/{userId}/deck")]
        public IHttpActionResult Get(int userId)
        {
            User user = _userRepository.FindById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Decks.Select(d => d.ToDto()));
        }

        // GET: api/user/5/deck/5
        [Route("api/user/{userId}/deck/{deckId}")]
        public IHttpActionResult Get(int userId, int deckId)
        {
            User user = _userRepository.FindById(userId);
            if (user == null)
            {
                return NotFound();
            }
            Deck deck = user.Decks.ElementAtOrDefault(deckId);
            if (deck == null)
            {
                return NotFound();
            }
            return Ok(deck.ToDto());
        }

        // POST: api/Deck
        [Route("api/user/{userId}/deck")]
        public IHttpActionResult Post(int userId, [FromBody]DeckDto deck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = _userRepository.FindById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var newDeck = DeckDtoToDeck(deck);
            if (!ValidateDeck(newDeck))
            {
                return BadRequest();
            }
            _userRepository.AddDeck(userId, newDeck);
            return Ok(newDeck.ToDto());
        }
        
        // PUT: api/Deck/5
        [Route("api/user/{userId}/deck/{deckId}")]
        public IHttpActionResult Put(int userId, int deckId, [FromBody]DeckDto deck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = _userRepository.FindById(userId);
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
            Deck newDeck = new Deck();
            newDeck.BattleKingCard = _cardRepository.Find(deck.BattleKingCard);
            newDeck.Faction = deck.Faction;
            newDeck.Cards = deck.Cards.Select(id => _cardRepository.Find(id)).ToList();
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
            
            // Not too many special cards
            int specialCardCount = deck.Cards.Count(c =>
            {
                var gwintType = c.GetGwintType();
                return gwintType.HasFlag(GwintType.Spell)
                    || gwintType.HasFlag(GwintType.RowModifier)
                    || gwintType.HasFlag(GwintType.GlobalEffect) 
                    || gwintType.HasFlag(GwintType.Weather);
            });
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
    }
}
