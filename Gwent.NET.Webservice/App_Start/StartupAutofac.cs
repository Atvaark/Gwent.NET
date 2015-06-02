using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Gwent.NET.Webservice.Auth;
using Owin;

namespace Gwent.NET.Webservice
{
    public partial class Startup
    {
        public static IContainer ConfigureAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<GwentModule>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterType<ApplicationUserManager>();
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacMiddleware(container);
            return container;
        }
    }
}