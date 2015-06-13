using System.Linq;
using System.Threading.Tasks;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Microsoft.AspNet.Identity;

namespace Gwent.NET.Webservice.Auth
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly IGwintContext _context;

        public UserStore(IGwintContext context)
        {
            _context = context;
        }


        public void Dispose()
        {

        }

        public Task CreateAsync(ApplicationUser user)
        {
            User newUser = _context.Users.Create();
            newUser.Name = user.UserName;
            newUser.PasswordHash = user.PasswordHash;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            user.Id = newUser.Id.ToString();
            return Task.FromResult(0);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            User existingUser = _context.Users.Find(user.Id);
            existingUser.Name = user.UserName;
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            User existingUser = _context.Users.Find(user.Id);
            _context.Users.Remove(existingUser);
            return _context.SaveChangesAsync();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var user = _context.Users.Find(int.Parse(userId));
            if (user == null)
            {
                return Task.FromResult<ApplicationUser>(null);
            }
            return Task.FromResult(new ApplicationUser
            {
                Id = user.Id.ToString(),
                UserName = user.Name
            });
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return Task.FromResult<ApplicationUser>(null);
            }
            return Task.FromResult(new ApplicationUser
            {
                Id = user.Id.ToString(),
                UserName = user.Name,
                PasswordHash = user.PasswordHash
            });
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}