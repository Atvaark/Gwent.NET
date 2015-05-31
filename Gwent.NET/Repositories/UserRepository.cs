using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<int, User> _users;
        private int _userId;
        private int _deckId;
        
        public UserRepository()
        {
            _users = new Dictionary<int, User>();
        }
        public User FindById(int id)
        {
            User user;
            _users.TryGetValue(id, out user);
            return user;
        }

        public User FindById(string id)
        {
            int userId;
            if (!int.TryParse(id, out userId))
            {
                return null;
            }
            return FindById(userId);
        }

        public User FindByName(string username)
        {
            return _users.Values.FirstOrDefault(u => u.Name == username);
        }

        public User Create(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            User newUser = new User
            {
                Id = Interlocked.Increment(ref _userId),
                Name = user.Name,
                Picture = "",
                PasswordHash = user.PasswordHash
            };
            _users.Add(newUser.Id, newUser);
            return newUser;
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

        public void AddDeck(int id, Deck deck)
        {
            User user = FindById(id);
            if (user == null)
            {
                throw new ArgumentException("id");
            }
            deck.Id = Interlocked.Increment(ref _deckId);
            user.Decks.Add(deck);
        }

        public void Delete(int id)
        {
            if (!_users.ContainsKey(id))
            {
                throw new ArgumentException("id");
            }
            _users.Remove(id);
        }

        public void Delete(string id)
        {
            int userId;
            if (!int.TryParse(id, out userId))
            {
                throw new ArgumentException("id");
            }
            Delete(userId);
        }
    }
}