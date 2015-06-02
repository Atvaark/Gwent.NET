using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace Gwent.NET.Webservice
{
    public partial class Startup
    {
        public static void ConfigureSignalR(IAppBuilder app, IContainer container)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJavaScriptProxies = true,
                    EnableDetailedErrors = true,
                    Resolver = new AutofacDependencyResolver(container)
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }

}