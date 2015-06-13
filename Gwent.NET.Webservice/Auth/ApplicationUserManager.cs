using Autofac;
using Autofac.Integration.Owin;
using Gwent.NET.Interfaces;
using Gwent.NET.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Gwent.NET.Webservice.Auth
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var gwintContext = new GwintContext();
            //var gwintContext = context.GetAutofacLifetimeScope().Resolve<IGwintContext>();
            var manager = new ApplicationUserManager(new UserStore(gwintContext));

            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true
            };
            // Konfigurieren der Überprüfungslogik für Kennwörter.
            manager.PasswordValidator = new PasswordValidator
            {
                //RequiredLength = 6
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}