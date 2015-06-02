using System.Web;
using System.Web.Http;

namespace Gwent.NET.Webservice
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
