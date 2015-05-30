using System.Web.Http;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Webservice.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        // GET: api/User/5
        public IHttpActionResult Get(int id)
        {
            User user = _userRepository.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToDto());
        }

        // POST: api/User
        public IHttpActionResult Post(string name)
        {
            User user = _userRepository.Create(name, "avatars/1.jpg");
            return Ok(user.ToDto());
        }
    }
}
