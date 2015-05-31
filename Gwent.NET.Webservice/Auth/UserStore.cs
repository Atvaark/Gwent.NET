using System.Threading.Tasks;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;
using Microsoft.AspNet.Identity;

namespace Gwent.NET.Webservice.Auth
{
    public class UserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly IUserRepository _userRepository;
        
        public UserStore(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public void Dispose()
        {

        }

        public Task CreateAsync(ApplicationUser user)
        {
            User newUser = new User
            {
                Name = user.UserName,
                PasswordHash = user.PasswordHash
            };
            newUser = _userRepository.Create(newUser);
            user.Id = newUser.Id.ToString();
            return Task.FromResult(0);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            var existingUser = _userRepository.FindById(user.Id);
            existingUser.Name = user.UserName;
            _userRepository.Update(existingUser.Id, existingUser);
            return Task.FromResult(0);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            _userRepository.Delete(user.Id);
            return Task.FromResult(0);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var user = _userRepository.FindById(userId);
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
            var user = _userRepository.FindByName(userName);
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