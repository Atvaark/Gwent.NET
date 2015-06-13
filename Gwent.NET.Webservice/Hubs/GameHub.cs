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
        private static readonly ConcurrentDictionary<string, string> UserIdConnectionIdDictionary = new ConcurrentDictionary<string, string>();
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
            UserIdConnectionIdDictionary.AddOrUpdate(userId, connectionId, (key, oldValue) => connectionId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var keys = UserIdConnectionIdDictionary.Where(kv => kv.Value == Context.ConnectionId).Select(kv => kv.Key);
            foreach (var key in keys)
            {
                string connectionId;
                UserIdConnectionIdDictionary.TryRemove(key, out connectionId);
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

        public string RecieveClientCommand(CommandDto commandDto)
        {
            Command command = CreateCommand(commandDto);
            int userId = int.Parse(Context.User.Identity.GetUserId());
            command.SenderUserId = userId;

            using (var context = _lifetimeScope.Resolve<IGwintContext>())
            {
                Game game = context.Games
                    .FirstOrDefault(g => g.IsActive && g.Players.Any(p => p.User.Id == userId));
                if (game == null)
                {
                    return "No running game found.";
                }
                try
                {
                    command.Execute(game);
                    context.SaveChanges();
                }
                catch (CommandException e)
                {
                    return e.Message;
                }
            }

            SendEvents(command.Events);
            return "";
        }

        private void SendEvents(IEnumerable<Event> gameEvents)
        {
            foreach (var gameEvent in gameEvents)
            {
                foreach (var recipient in gameEvent.Recipients)
                {
                    string connectionId;
                    if (UserIdConnectionIdDictionary.TryGetValue(recipient.ToString(), out connectionId))
                    {
                        Clients.Client(connectionId).recieveServerEvent(gameEvent);
                    }
                }
            }
        }
        
        private Command CreateCommand(CommandDto commandDto)
        {
            if (commandDto == null) throw new ArgumentNullException("commandDto");
            switch (commandDto.Type)
            {
                case CommandType.StartGame:
                    return new StartGameCommand();
                case CommandType.RedrawCard:
                    if (!commandDto.CardId.HasValue)
                    {
                        throw new ArgumentException();
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
                        throw new ArgumentException();
                    }
                    return new PickStartingPlayerCommand
                    {
                        StartingPlayerId = commandDto.StartPlayerId.Value
                    };
                case CommandType.PlayCard:
                    if (!commandDto.CardId.HasValue || !commandDto.Slot.HasValue)
                    {
                        throw new ArgumentException();
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
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}