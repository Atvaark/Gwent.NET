using System.Threading.Tasks;
using Autofac;
using Gwent.NET.Webservice.Auth;
using Microsoft.AspNet.SignalR;

namespace Gwent.NET.Webservice.Hubs
{
    [QueryStringBearerAuthorize]
    public class GameHub : Hub
    {
        private readonly ILifetimeScope _lifetimeScope;

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

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void RecieveClientCommand(string message)
        {

        }
    }
}