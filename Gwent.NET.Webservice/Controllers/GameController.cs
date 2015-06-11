using System.Linq;
using System.Web.Http;
using Gwent.NET.DTOs;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.States;

namespace Gwent.NET.Webservice.Controllers
{
    public class GameController : AuthenticatedApiController
    {
        private readonly IGameRepository _gameRepository;
        private readonly ICardRepository _cardRepository;

        public GameController(IUserRepository userRepository, IGameRepository gameRepository, ICardRepository cardRepository) : base(userRepository)
        {
            _gameRepository = gameRepository;
            _cardRepository = cardRepository;
        }

        [HttpGet]
        [Route("api/game/browse")]
        public IHttpActionResult Browse()
        {
            var games = _gameRepository.Get().Where(g => g.State.IsJoinable).Select(g => new GameBrowseDto
            {
                Id = g.Id,
                State = g.State.GetType().Name,
                PlayerCount = g.Players.Count
            });
            return Ok(games);
        }

        // GET: api/Game/Active
        [HttpGet]
        [Route("api/game/active")]
        public IHttpActionResult Active()
        {
            User user;
            if (!TryGetUser(out user))
            {
                return BadRequest();
            }

            var game = _gameRepository.FindByUserId(user.Id).FirstOrDefault(g => !g.State.IsOver);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game.ToDto());
        }

        // GET: api/Game/5
        public IHttpActionResult Get(int id)
        {
            var game = _gameRepository.Find(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game.ToDto());
        }

        // POST: api/Game
        public IHttpActionResult Post()
        {
            User user;
            if (!TryGetUser(out user))
            {
                return BadRequest();
            }

            Game game = new Game
            {
                State = new LobbyState(),
                Players =
                {
                    new Player
                    {
                        User = user,
                        IsOwner = true,
                        Deck = GenerateDemoDeck() // TODO: Read the default player deck.
                    }
                }
            };
            game = _gameRepository.Create(game);
            return Ok(game.ToDto());
        }

        private Deck GenerateDemoDeck()
        {
            var demoDeck = new Deck
            {
                Id = 1,
                Faction = GwentFaction.Scoiatael,
                BattleKingCard = _cardRepository.Find(3002),
                Cards =
                {
                    _cardRepository.Find(0),
                    _cardRepository.Find(0),
                    _cardRepository.Find(1),
                    _cardRepository.Find(2),
                    _cardRepository.Find(3),
                    _cardRepository.Find(4),
                    _cardRepository.Find(5),
                    _cardRepository.Find(6),
                    _cardRepository.Find(7),
                    _cardRepository.Find(8),
                    _cardRepository.Find(9),
                    _cardRepository.Find(10),
                    _cardRepository.Find(11),
                    _cardRepository.Find(12),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                    _cardRepository.Find(306),
                }
            };
            return demoDeck;
        }

        // PUT: api/Game/5/Join
        [HttpPut]
        [Route("api/game/{id}/join")]
        public IHttpActionResult Join(int id)
        {
            User user;
            if (!TryGetUser(out user))
            {
                return BadRequest();
            }
            var game = _gameRepository.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            if (_gameRepository.FindByUserId(user.Id).Any(g => !g.State.IsOver))
            {
                return BadRequest();
            }

            game.Players.Add(new Player
            {
                User = user,
                Deck = GenerateDemoDeck() // TODO: Read the default player deck.

            });
            return Ok(game.ToDto());
        }

    }
}
