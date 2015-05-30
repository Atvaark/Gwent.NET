using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<int, User> _users;
        private int _id;
        
        public UserRepository()
        {
            _users = new Dictionary<int, User>();
        }
        public User Find(int id)
        {
            User user;
            _users.TryGetValue(id, out user);
            return user;
        }

        public User Create(string name, string picture)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (picture == null) throw new ArgumentNullException("picture");
            User user = new User
            {
                Id = Interlocked.Increment(ref _id),
                Name = name,
                Picture = picture
            };
            _users.Add(user.Id, user);
            return user;
        }

        public void Update(int id, User user)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (!_users.ContainsKey(id) || user.Id != id)
            {
                throw new ArgumentException("id");
            }
            _users[id] = user;
        }
    }
}