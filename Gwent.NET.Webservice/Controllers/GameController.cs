using System.Web.Http;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.States;

namespace Gwent.NET.Webservice.Controllers
{
    public class GameController : ApiController
    {
        private readonly IGameRepository _gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
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
            Game game = new Game
            {
                State = new LobbyState()
            };
            game = _gameRepository.Create(game);
            return Ok(game.ToDto());
        }

        // PUT: api/Game/5/Join
        [HttpPut]
        [Route("api/game/{id}/join")]
        public IHttpActionResult Join(int id)
        {
            var game = _gameRepository.Find(id);
            if (game == null)
            {
                return NotFound();
            }


            
            return Ok(game.ToDto());
        }
    }
}
