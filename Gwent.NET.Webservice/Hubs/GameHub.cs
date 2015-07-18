using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Gwent.NET.Commands;
using Gwent.NET.DTOs;
using Gwent.NET.Events;
using Gwent.NET.Exceptions;
using Gwent.NET.Extensions;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.Model.Enums;
using Gwent.NET.Model.States;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace Gwent.NET.Webservice.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly ILifetimeScope _lifetimeScope;

        private long UserId
        {
            get { return long.Parse(Context.User.Identity.GetUserId()); }
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
            var userConnectionMapping = _lifetimeScope.Resolve<IUserConnectionMap>();
            userConnectionMapping.Connect(Context.ConnectionId, Context.User.Identity.GetUserId());

            return base.OnConnected();
        }
        
        public override Task OnDisconnected(bool stopCalled)
        {
            var userConnectionMapping = _lifetimeScope.Resolve<IUserConnectionMap>();
            userConnectionMapping.Disconnect(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
        
        public override Task OnReconnected()
        {
            return base.OnReconnected();
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
            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                long userId = UserId;
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
            var battleKingCard = context.Cards.Find(3002L);
            var cards = new List<Card>()
            {
                context.Cards.Find(0L),
                context.Cards.Find(0L),
                context.Cards.Find(1L),
                context.Cards.Find(2L),
                context.Cards.Find(3L),
                context.Cards.Find(4L),
                context.Cards.Find(5L),
                context.Cards.Find(6L),
                context.Cards.Find(7L),
                context.Cards.Find(8L),
                context.Cards.Find(9L),
                context.Cards.Find(10L),
                context.Cards.Find(11L),
                context.Cards.Find(12L),
                context.Cards.Find(306L),
                context.Cards.Find(306L),
                context.Cards.Find(306L),
                context.Cards.Find(306L),
                context.Cards.Find(306L),
                context.Cards.Find(306L),
                context.Cards.Find(306L),
                context.Cards.Find(306L),
                context.Cards.Find(306L)
            };

            var demoDeck = new Deck
            {
                Faction = GwintFaction.Scoiatael,
                BattleKingCard = battleKingCard.ToDeckCard(),
                Cards = cards.Select(c => c.ToDeckCard()).ToList(),
                IsPrimaryDeck = true
            };
            return demoDeck;
        }

        public GameHubResult<GameDto> JoinGame(long gameId)
        {
            var retryJoinGameCount = Constants.RetryJoinGameCount;
            while (retryJoinGameCount > 0)
            {
                try
                {
                    return ExecuteJoinGame(gameId);
                }
                catch (OptimisticConcurrencyException)
                {
                    retryJoinGameCount--;
                }
            }

            return new GameHubResult<GameDto>
            {
                Error = string.Format("Hub: Unable to join game after {0} tries", Constants.RetryExecuteCommandCount)
            };
        }

        private GameHubResult<GameDto> ExecuteJoinGame(long gameId)
        {
            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == UserId);
                if (user == null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: User not found"
                    };
                }
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

                var player = context.Players.Create();
                player.User = user;
                player.Deck = primaryDeck;
                game.Players.Add(player);
                context.SaveChanges();

                var opponentUserId = game.GetOpponentPlayerByUserId(user.Id).User.Id;
                var playerJoinedEvent = new PlayerJoinedEvent(opponentUserId)
                {
                    Game = game.ToPersonalizedDto(opponentUserId)
                };
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
                long senderUserId = UserId;
                command.SenderUserId = senderUserId;

                int retryCount = Constants.RetryExecuteCommandCount;
                while (retryCount > 0)
                {
                    try
                    {
                        return ExecuteClientCommand(command);
                    }
                    catch (OptimisticConcurrencyException)
                    {
                        retryCount--;
                    }
                }

                return new GameHubResult<GameDto>
                {
                    Error = string.Format("Hub: Unable to execute command after {0} tries", Constants.RetryExecuteCommandCount)
                };
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

        private GameHubResult<GameDto> ExecuteClientCommand(Command command)
        {
            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                Game game = GetActiveGameByUserId(context, command.SenderUserId);
                if (game == null)
                {
                    return new GameHubResult<GameDto>
                    {
                        Error = "Hub: No running game found."
                    };
                }

                command.Execute(game);
                command.TransitionToNextState(game);
                context.SaveChanges();
                DispatchEvents(command.Events);
                return new GameHubResult<GameDto>();
            }
        }

        private Game GetActiveGameByUserId(IGwintContext context, long userId)
        {
            return context.Games.FirstOrDefault(g => g.IsActive && g.Players.Any(p => p.User.Id == userId));
        }

        private void DispatchEvents(IEnumerable<Event> gameEvents)
        {
            foreach (var gameEvent in gameEvents)
            {
                foreach (var recipient in gameEvent.Recipients)
                {
                    var userConnectionMapping = _lifetimeScope.Resolve<IUserConnectionMap>();
                    foreach (var connectionId in userConnectionMapping.GetConnections(recipient.ToString()))
                    {
                        dynamic client = Clients.Client(connectionId);
                        client.recieveServerEvent(gameEvent);
                    }
                }
            }
        }

        private Command CreateCommand(CommandDto commandDto) // TODO: Move to a command factory class
        {
            if (commandDto == null) throw new CommandParseException("Unknown command");
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
                    if (!commandDto.StartingPlayerUserId.HasValue)
                    {
                        throw new CommandParseException();
                    }
                    return new PickStartingPlayerCommand
                    {
                        StartingPlayerUserId = commandDto.StartingPlayerUserId.Value
                    };
                case CommandType.PlayCard:
                    if (!commandDto.CardId.HasValue || !commandDto.Slot.HasValue)
                    {
                        throw new CommandParseException();
                    }
                    return new PlayCardCommand
                    {
                        CardId = commandDto.CardId.Value,
                        TargetCardId = commandDto.ResurrectCardId,
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