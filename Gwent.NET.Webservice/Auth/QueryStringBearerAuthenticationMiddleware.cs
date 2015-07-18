using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace Gwent.NET.Webservice.Auth
{
    public class QueryStringBearerAuthenticationMiddleware : OwinMiddleware
    {
        private readonly OAuthAuthorizationServerOptions _options;
        private readonly PathString _path;

        public QueryStringBearerAuthenticationMiddleware(OwinMiddleware next, OAuthAuthorizationServerOptions options, string path)
            : base(next)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (path == null) throw new ArgumentNullException("path");
            _options = options;
            _path = new PathString(path);
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.StartsWithSegments(_path))
            {
                var token = context.Request.Query["Bearer"];
                if (token != null)
                {
                    var ticket = _options.AccessTokenFormat.Unprotect(token);
                    if (ticket != null && ticket.Identity != null && ticket.Identity.IsAuthenticated)
                    {
                        context.Request.User = new ClaimsPrincipal(ticket.Identity);
                        context.Request.Environment["server.User"] = new ClaimsPrincipal(ticket.Identity);
                        context.Request.Environment["server.Username"] = ticket.Identity.Name;
                    }
                }
            }
            await Next.Invoke(context);
        }
    }
}