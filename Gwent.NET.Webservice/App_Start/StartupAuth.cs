using System;
using Autofac;
using Gwent.NET.Webservice.Auth;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Gwent.NET.Webservice
{
    public partial class Startup
    {
        public static string PublicClientId { get; set; }
        public static OAuthAuthorizationServerOptions OAuthOptions { get; set; }

        public static void ConfigureAuth(IAppBuilder app, IContainer container)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            var cookieAuthenticationOptions = new CookieAuthenticationOptions
            {
                //CookieHttpOnly = false
            };
            app.UseCookieAuthentication(cookieAuthenticationOptions);
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/user/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // TODO: Remove the following line before you deploy to production:
                AllowInsecureHttp = true,
                AccessTokenFormat = new TicketDataFormat(container.Resolve<IDataProtector>())
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
            app.UseQueryStringOAuthBearerTokens(OAuthOptions, SignalRPath);
        }

    }
}