using System;
using System.Linq;
using Gwent.NET.Interfaces;
using Gwent.NET.Model;

namespace Gwent.NET.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IGwintContext _context;

        public UserRepository(IGwintContext gwintContext)
        {
            _context = gwintContext;
        }


        public User FindById(int id)
        {
            return _context.Users.Find(id);
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
            return _context.Users.FirstOrDefault(u => u.Name == username);
        }

        public User CreateUser(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            var newUser = _context.Users.Create();
            newUser.Name = user.Name;
            newUser.Picture = "";
            newUser.PasswordHash = user.PasswordHash;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public void Update(int id, User user)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser == null)
            {
                throw new ArgumentException("id");
            }
            existingUser.Name = user.Name;
            existingUser.Picture = user.Picture;
            existingUser.PasswordHash = user.PasswordHash;
            _context.SaveChanges();
        }

        public void AddDeck(int id, Deck deck)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                throw new ArgumentException("id");
            }

            var newDeck = _context.Decks.Create();
            newDeck.Faction = deck.Faction;
            newDeck.BattleKingCard = deck.BattleKingCard;
            newDeck.Cards.AddRange(deck.Cards); // BUG: Thanks to the composite primary key only one of each card type can be saved.
            newDeck.IsPrimaryDeck = !user.Decks.Any();
            user.Decks.Add(newDeck);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                throw new ArgumentException("id");
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
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

        public Player CreatePlayer(User user)
        {
            Player player = _context.Players.Create();
            player.User = user;
            player.Deck = user.Decks.FirstOrDefault(d => d.IsPrimaryDeck);
            _context.SaveChanges();
            return player;
        }
    }
}