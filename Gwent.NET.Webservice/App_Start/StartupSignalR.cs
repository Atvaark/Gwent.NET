using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
                    EnableJavaScriptProxies = false,
                    EnableDetailedErrors = true,
                    Resolver = new AutofacDependencyResolver(container)
                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }

}