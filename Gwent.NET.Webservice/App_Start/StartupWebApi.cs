using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Gwent.NET.Webservice
{
    public partial class Startup
    {

        public static void ConfigureWebApi(IAppBuilder app)
        {
            var config = GlobalConfiguration.Configuration;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }
}

