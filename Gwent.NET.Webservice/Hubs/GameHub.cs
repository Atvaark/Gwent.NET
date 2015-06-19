using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Gwent.NET.Commands;
using Gwent.NET.DTOs;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.Model.States;
using Gwent.NET.Webservice.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace Gwent.NET.Webservice.Hubs
{
    [QueryStringBearerAuthorize]
    public class GameHub : Hub
    {
        private readonly ILifetimeScope _lifetimeScope;
        // TODO: Use a bi-directional dictionary instead or a repository
        private static readonly ConcurrentDictionary<string, string> UserIdToConnectionIdDictionary = new ConcurrentDictionary<string, string>();

        private int UserId
        {
            get { return int.Parse(Context.User.Identity.GetUserId()); }
        }

        public GameHub(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope.BeginLifetimeScope();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _lifetimeScope != null)
            {
                _lifetimeScope.Dispose();
            }
            base.Dispose(disposing);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        private void MapConnectionIdToUserId()
        {
            string userId = Context.User.Identity.GetUserId();
            var connectionId = Context.ConnectionId;
            UserIdToConnectionIdDictionary.AddOrUpdate(userId, connectionId, (key, oldValue) => connectionId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var keys = UserIdToConnectionIdDictionary.Where(kv => kv.Value == Context.ConnectionId).Select(kv => kv.Key);
            foreach (var key in keys)
            {
                string connectionId;
                UserIdToConnectionIdDictionary.TryRemove(key, out connectionId);
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void Authenticate()
        {
            MapConnectionIdToUserId();
        }

        public GameHubResult<ICollection<GameBrowseDto>> BrowseGames()
        {
            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                return new GameHubResult<ICollection<GameBrowseDto>>
                {
                    Data = context.Games
                    .Where(g => g.IsActive)
                    .Select(g => new
                    {
                        Id = g.Id,
                        State = g.State,
                        PlayerCount = g.Players.Count
                    })
                    .AsEnumerable()
                    .Select(g => new GameBrowseDto
                    {
                        Id = g.Id,
                        State = g.State.Name,
                        PlayerCount = g.PlayerCount
                    })
                    .ToList()
                };
            }
        }
        
        public GameHubResult<GameDto> GetActiveGame()
        {
            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                var userId = UserId;
                var game = GetActiveGameByUserId(context, userId);
                if (game == null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: No active game found."
                    };
                }
                return new GameHubResult<GameDto>
                {
                    Data = game.ToPersonalizedDto(userId)
                };
            }
        }


        public GameHubResult<GameDto> CreateGame()
        {
            int userId = UserId;

            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: User not found"
                    };
                }

                var activeGame = GetActiveGameByUserId(context, userId);
                if (activeGame != null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: Game still running."
                    };
                }

                var primaryDeck = GenerateDemoDeck(context);
                //var primaryDeck = user.Decks.FirstOrDefault(d => d.IsPrimaryDeck); // TODO: Renable picking the primary deck
                if (primaryDeck == null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: No deck found."
                    };
                }
                
                var player = context.Players.Create();
                player.User = user;
                player.Deck = primaryDeck;
                player.IsOwner = true;
                player.IsTurn = true;

                var game = context.Games.Create();
                game.IsActive = true;
                game.State = new LobbyState();
                game.Players.Add(player);
                context.Games.Add(game);
                context.SaveChanges();
                return new GameHubResult<GameDto>
                {
                    Data = game.ToPersonalizedDto(userId)
                };
            }
        }

        private Deck GenerateDemoDeck(IGwintContext context)
        {
            var demoDeck = new Deck
            {
                Faction = GwentFaction.Scoiatael,
                BattleKingCard = context.Cards.Find(3002),
                Cards =
                {
                    context.Cards.Find(0),
                    context.Cards.Find(0),
                    context.Cards.Find(1),
                    context.Cards.Find(2),
                    context.Cards.Find(3),
                    context.Cards.Find(4),
                    context.Cards.Find(5),
                    context.Cards.Find(6),
                    context.Cards.Find(7),
                    context.Cards.Find(8),
                    context.Cards.Find(9),
                    context.Cards.Find(10),
                    context.Cards.Find(11),
                    context.Cards.Find(12),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                    context.Cards.Find(306),
                },
                IsPrimaryDeck = true
            };
            return demoDeck;
        }

        public GameHubResult<GameDto> JoinGame(int gameId)
        {
            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == UserId);
                var game = context.Games.Find(gameId);
                if (game == null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: Game not found."
                    };
                }

                if (game.Players.Count == Constants.MaxPlayerCount)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: Game is full."
                    };
                }

                if (GetActiveGameByUserId(context, user.Id) != null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: Other game still running."
                    };
                }

                var primaryDeck = GenerateDemoDeck(context);
                //var primaryDeck = user.Decks.FirstOrDefault(d => d.IsPrimaryDeck); // TODO: Renable picking the primary deck
                if (primaryDeck == null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: No deck found."
                    };
                }

                var playerJoinedEvent = new PlayerJoinedEvent(game.GetAllUserIds());
                var player = context.Players.Create();
                player.User = user;
                player.Deck = primaryDeck;
                game.Players.Add(player);
                context.SaveChanges();
                DispatchEvents(new Event[] { playerJoinedEvent });
                return new GameHubResult<GameDto>
                {
                    Data = game.ToPersonalizedDto(user.Id)
                };
            }
        }

        public GameHubResult<GameDto> RecieveClientCommand(CommandDto commandDto)
        {
            try
            {
                Command command = CreateCommand(commandDto);
                int userId = UserId;
                command.SenderUserId = userId;

                using (var context = _lifetimeScope.Resolve<IGwintContext>())
                {
                    Game game = GetActiveGameByUserId(context, userId);
                    if (game == null)
                    {
                        return new GameHubResult<GameDto>
                        {
                            Error = "Hub: No running game found."
                        };
                    }

                    command.Execute(game);
                    context.SaveChanges();
                }

                DispatchEvents(command.Events);
                return new GameHubResult<GameDto>();
            }
            catch (Exception e)
            {
                GwintException gwintException = e as GwintException;
                if (gwintException != null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = e.Message
                    };
                }

                return new GameHubResult<GameDto>
                {
                    Error = "Hub: Internal server error"
                };
            }
        }

        private static Game GetActiveGameByUserId(IGwintContext context, int userId)
        {
            return context.Games.FirstOrDefault(g => g.IsActive && g.Players.Any(p => p.User.Id == userId));
        }

        private void DispatchEvents(IEnumerable<Event> gameEvents)
        {
            foreach (var gameEvent in gameEvents)
            {
                foreach (var recipient in gameEvent.Recipients)
                {
                    string connectionId;
                    if (UserIdToConnectionIdDictionary.TryGetValue(recipient.ToString(), out connectionId))
                    {
                        Clients.Client(connectionId).recieveServerEvent(gameEvent);
                    }
                }
            }
        }

        private Command CreateCommand(CommandDto commandDto) // TODO: Move to a command factory class
        {
            if (commandDto == null) throw new ArgumentNullException("commandDto");
            switch (commandDto.Type)
            {
                case CommandType.StartGame:
                    return new StartGameCommand();
                case CommandType.RedrawCard:
                    if (!commandDto.CardId.HasValue)
                    {
                        throw new CommandParseException();
                    }
                    return new RedrawCardCommand
                    {
                        CardId = commandDto.CardId.Value
                    };
                case CommandType.EndRedrawCard:
                    return new EndRedrawCardCommand();
                case CommandType.ForfeitGame:
                    return new ForfeitGameCommand();
                case CommandType.Pass:
                    return new PassCommand();
                case CommandType.PickStartingPlayer:
                    if (!commandDto.StartPlayerId.HasValue)
                    {
                        throw new CommandParseException();
                    }
                    return new PickStartingPlayerCommand
                    {
                        StartingPlayerId = commandDto.StartPlayerId.Value
                    };
                case CommandType.PlayCard:
                    if (!commandDto.CardId.HasValue || !commandDto.Slot.HasValue)
                    {
                        throw new CommandParseException();
                    }
                    return new PlayCardCommand
                    {
                        CardId = commandDto.CardId.Value,
                        ResurrectCardId = commandDto.ResurrectCardId,
                        Slot = commandDto.Slot.Value
                    };
                case CommandType.UseBattleKingCard:
                    return new UseBattleKingCardCommand();
                default:
                    throw new CommandParseException("Unknown command");
            }
        }
    }
}