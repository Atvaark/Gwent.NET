using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Microsoft.Owin.Security;

namespace Gwent.NET.Webservice.Controllers
{
    public abstract class AuthenticatedApiController : ApiController
    {
        protected readonly IGwintContext Context;

        protected AuthenticatedApiController(IGwintContext context)
        {
            Context = context;
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        protected bool TryGetUser(out User user)
        {
            user = null;
            int userId;
            if (!TryGetUserId(out userId))
            {
                return false;
            }
            user = Context.Users.Find(userId);
            return user != null;
        }

        protected bool TryGetUserId(out int userId)
        {
            userId = 0;
            var user = Authentication.User;
            if (user == null)
            {
                return false;
            }

            var nameIdentifierClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null)
            {
                return false;
            }

            return int.TryParse(nameIdentifierClaim.Value, out userId);
        }
    }
}