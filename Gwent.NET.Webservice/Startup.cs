using Gwent.NET.Webservice;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Gwent.NET.Webservice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigureAutofac(app);
            ConfigureAuth(app);
            ConfigureSignalR(app, container);
            //ConfigureWebApi(app);
        }
    }
}