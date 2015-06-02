using System.Security.Claims;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Owin;
using Microsoft.Owin.Security.DataHandler;

namespace Gwent.NET.Webservice.Auth
{
    public class QueryStringBearerAuthorizeAttribute : AuthorizeAttribute
    {
        private const string AuthenticationType = "Bearer";

        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            // MachineKeyProtector when hosting in IIS and DpapiDataProtectionProvider when selfhosting
            var dataProtectionProvider = new MachineKeyProtector();
            var secureDataFormat = new TicketDataFormat(dataProtectionProvider);
            var token = request.QueryString.Get(AuthenticationType);
            var ticket = secureDataFormat.Unprotect(token);
            if (ticket != null && ticket.Identity != null && ticket.Identity.IsAuthenticated)
            {
                request.Environment["server.User"] = new ClaimsPrincipal(ticket.Identity);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var connectionId = hubIncomingInvokerContext.Hub.Context.ConnectionId;
            var environment = hubIncomingInvokerContext.Hub.Context.Request.Environment;
            var principal = environment["server.User"] as ClaimsPrincipal;
            if (principal != null && principal.Identity != null && principal.Identity.IsAuthenticated)
            {
                hubIncomingInvokerContext.Hub.Context = new HubCallerContext(new ServerRequest(environment), connectionId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}