using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Gwent.NET.DTOs;
using Gwent.NET.Extensions;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Gwent.NET.Webservice.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace Gwent.NET.Webservice.Controllers
{
    public class UserController : AuthenticatedApiController
    {
        private ApplicationUserManager _userManager;

        public UserController(IGwintContext context)
            : base(context)
        {

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        // GET: api/User/5
        [Authorize]
        public IHttpActionResult Get(int id)
        {
            User user = Context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToDto());
        }

        // GET: api/User/Me
        [Authorize]
        [HttpGet]
        [Route("api/user/me")]
        public IHttpActionResult Me()
        {
            int userId;
            if (!TryGetUserId(out userId))
            {
                return BadRequest();
            }
            User user = Context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToDto());
        }

        // POST api/User/Logout
        [Authorize]
        [HttpPost]
        [Route("api/user/logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST: api/User/Register
        [HttpPost]
        [Route("api/user/register")]
        public async Task<IHttpActionResult> Register([FromBody]RegistrationDto registration)
        {
            if (!ModelState.IsValid || registration == null)
            {
                return BadRequest(ModelState);
            }
            var applicationUser = new ApplicationUser
            {
                UserName = registration.Username
            };
            IdentityResult result = await UserManager.CreateAsync(applicationUser, registration.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            User user = Context.Users.Find(int.Parse(applicationUser.Id));
            return Ok(user.ToDto());
        }

        // POST api/User/ChangePassword
        [Authorize]
        [Route("api/user/changepassword")]
        public async Task<IHttpActionResult> ChangePassword(PasswordChangeDto passwordChange)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(
                User.Identity.GetUserId(),
                passwordChange.OldPassword,
                passwordChange.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}
