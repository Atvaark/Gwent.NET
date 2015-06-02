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

        public GameController(IUserRepository userRepository, IGameRepository gameRepository) : base(userRepository)
        {
            _gameRepository = gameRepository;
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
                        IsOwner = true
                    }
                }
            };
            game = _gameRepository.Create(game);
            return Ok(game.ToDto());
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
                User = user
            });
            return Ok(game.ToDto());
        }

    }
}
