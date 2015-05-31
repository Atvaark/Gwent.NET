using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Gwent.NET.Webservice.Auth;
using Owin;

namespace Gwent.NET.Webservice
{
    public partial class Startup
    {
        public static void ConfigureAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterModule<GwentModule>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ApplicationUserManager>();
            
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacMiddleware(container);
        }
    }
}