using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Gwent.NET.DTOs;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.Model.States;

namespace Gwent.NET.Webservice.Controllers
{
    public class GameController : AuthenticatedApiController
    {
        public GameController(IGwintContext context)
            : base(context)
        {

        }

        [HttpGet]
        [Route("api/game/browse")]
        public IHttpActionResult Browse()
        {
            var games = Context.Games
                .Where(g => g.IsActive)
                .AsEnumerable()
                .Select(g => new GameBrowseDto
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

            var game = Context.Games
                .Include(g => g.Players)
                .FirstOrDefault(g => g.IsActive && g.Players.Any(p => p.User.Id == user.Id));
            if (game == null)
            {
                return new StatusCodeResult(HttpStatusCode.NoContent, this);
            }
            return Ok(game.ToDto().StripOpponentPrivateInfo(user.Id));
        }

        // GET: api/Game/5
        public IHttpActionResult Get(int id)
        {
            var game = Context.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game.ToDto().StripPrivateInfo());
        }

        // POST: api/Game
        public IHttpActionResult Post()
        {
            User user;
            if (!TryGetUser(out user))
            {
                return BadRequest();
            }

            var primaryDeck = GenerateDemoDeck();
            //var primaryDeck = user.Decks.FirstOrDefault(d => d.IsPrimaryDeck); // TODO: Renable picking the primary deck
            if (primaryDeck == null)
            {
                return BadRequest();
            }


            var player = Context.Players.Create();
            player.User = user;
            player.Deck = primaryDeck;
            player.IsOwner = true;

            var game = Context.Games.Create();
            game.IsActive = true;
            game.State = new LobbyState();
            game.Players.Add(player);
            Context.Games.Add(game);
            Context.SaveChanges();
            return Ok(game.ToDto().StripOpponentPrivateInfo(user.Id));
        }

        private Deck GenerateDemoDeck()
        {
            var demoDeck = new Deck
            {
                Faction = GwentFaction.Scoiatael,
                BattleKingCard = Context.Cards.Find(3002),
                Cards =
                {
                    Context.Cards.Find(0),
                    Context.Cards.Find(0),
                    Context.Cards.Find(1),
                    Context.Cards.Find(2),
                    Context.Cards.Find(3),
                    Context.Cards.Find(4),
                    Context.Cards.Find(5),
                    Context.Cards.Find(6),
                    Context.Cards.Find(7),
                    Context.Cards.Find(8),
                    Context.Cards.Find(9),
                    Context.Cards.Find(10),
                    Context.Cards.Find(11),
                    Context.Cards.Find(12),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                    Context.Cards.Find(306),
                },
                IsPrimaryDeck = true
            };
            return demoDeck;
        }

        // PUT: api/Game/5/Join
        [HttpPut]
        [Route("api/game/{id}/join")]
        public IHttpActionResult Join(int id)
        {
            // TODO: Join via SignalR. That way the other player can be notified right away.
            User user;
            if (!TryGetUser(out user))
            {
                return BadRequest();
            }
            var game = Context.Games.Find(id);
            if (game == null)
            {
                return NotFound();
            }

            if (Context.Games
                .Where(g => g.IsActive)
                .Any(g => g.Players.Any(p => p.User.Id == user.Id)))
            {
                return BadRequest();
            }

            var primaryDeck = GenerateDemoDeck();
            //var primaryDeck = user.Decks.FirstOrDefault(d => d.IsPrimaryDeck); // TODO: Renable picking the primary deck
            if (primaryDeck == null)
            {
                return BadRequest();
            }

            var player = Context.Players.Create();
            player.User = user;
            player.Deck = primaryDeck;
            game.Players.Add(player);
            Context.SaveChanges();
            return Ok(game.ToDto().StripOpponentPrivateInfo(user.Id));
        }
    }
}
