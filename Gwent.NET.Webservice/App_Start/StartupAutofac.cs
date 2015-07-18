using System;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Gwent.NET.Webservice.Auth;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin.Security.DataProtection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            builder.Register(context =>
            {
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new SignalRContractResolver();
                return JsonSerializer.Create(settings);
            }).As<JsonSerializer>();
            builder.RegisterType<MachineKeyProtector>().As<IDataProtector>();

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            app.UseAutofacMiddleware(container);
            return container;
        }

        public class SignalRContractResolver : IContractResolver
        {

            private readonly Assembly _assembly;
            private readonly IContractResolver _camelCaseContractResolver;
            private readonly IContractResolver _defaultContractSerializer;

            public SignalRContractResolver()
            {
                _defaultContractSerializer = new DefaultContractResolver();
                _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
                _assembly = typeof(Connection).Assembly;
            }

            public JsonContract ResolveContract(Type type)
            {
                if (type.Assembly.Equals(_assembly))
                {
                    return _defaultContractSerializer.ResolveContract(type);

                }

                return _camelCaseContractResolver.ResolveContract(type);
            }

        }
    }
}