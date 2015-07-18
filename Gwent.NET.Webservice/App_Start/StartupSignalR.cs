using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace Gwent.NET.Webservice
{
    public partial class Startup
    {
        private static string SignalRPath = "/signalr";

        public static void ConfigureSignalR(IAppBuilder app, IContainer container)
        {
            app.Map(SignalRPath, map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJavaScriptProxies = false,
                    EnableDetailedErrors = true,
                    Resolver = new AutofacDependencyResolver(container)
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }

}