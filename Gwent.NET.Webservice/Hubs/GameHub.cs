using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Autofac;
using Gwent.NET.Commands;
using Gwent.NET.DTOs;
using Gwent.NET.Webservice.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace Gwent.NET.Webservice.Hubs
{
    [QueryStringBearerAuthorize]
    public class GameHub : Hub
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ConcurrentDictionary<string, string> _userIdConnectionIdDictionary;

        public GameHub(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope.BeginLifetimeScope();
            _userIdConnectionIdDictionary = new ConcurrentDictionary<string, string>();
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
            var testCommand = new CommandDto
            {
                Type = CommandType.None
            };
            Clients.Caller.recieveServerCommand(testCommand);
            return base.OnConnected();
        }

        private void MapConnectionIdToUserId()
        {
            string userId = Context.User.Identity.GetUserId();
            var connectionId = Context.ConnectionId;
            _userIdConnectionIdDictionary.AddOrUpdate(connectionId, userId, (key, oldValue) => userId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userId;
            _userIdConnectionIdDictionary.TryRemove(Context.ConnectionId, out userId);
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
        
        public void RecieveClientCommand(CommandDto commandDto)
        {
            if (commandDto == null)
            {
                throw new ArgumentException();
            }

            try
            {
                Command command = CreateCommand(commandDto);
            }
            catch (ArgumentException argumentException)
            {
                throw;
            }
            // TODO: Move the command in a pipeline or execute it.
        }


        private Command CreateCommand(CommandDto commandDto)
        {
            switch (commandDto.Type)
            {
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
                        StartPlayerId = commandDto.StartPlayerId.Value
                    };
                case CommandType.PlayCard:
                    if (!commandDto.CardId.HasValue || !commandDto.Slot.HasValue)
                    {
                        throw new ArgumentException();
                    }
                    return new PlayCardCommand
                    {
                        CardId = commandDto.CardId.Value,
                        Slot = commandDto.Slot.Value
                    };
                case CommandType.RedrawCard:
                    if (!commandDto.CardId.HasValue)
                    {
                        throw new ArgumentException();
                    }
                    return new RedrawCardCommand
                    {
                        CardId = commandDto.CardId.Value
                    };
                case CommandType.Resurrect:
                    if (!commandDto.CardId.HasValue)
                    {
                        throw new ArgumentException();
                    }
                    return new ResurrectCommand
                    {
                        CardId = commandDto.CardId.Value
                    };
                case CommandType.UseBattleKingCard:
                    return new UseBattleKingCardCommand();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}