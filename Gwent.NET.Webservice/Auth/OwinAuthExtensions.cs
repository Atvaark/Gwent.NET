using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Gwent.NET.Webservice.Auth
{
    public static class OwinAuthExtensions
    {
        public static void UseQueryStringOAuthBearerTokens(this IAppBuilder app, OAuthAuthorizationServerOptions options, string path)
        {
            app.Use<QueryStringBearerAuthenticationMiddleware>(options, path);
        }
    }
}