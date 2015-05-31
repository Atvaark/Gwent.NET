using Gwent.NET.Model;

namespace Gwent.NET.Interfaces
{
    public interface IUserRepository
    {
        User FindById(int id);
        User FindById(string id);
        User FindByName(string username);
        User Create(User user);
        void Update(int id, User user);
        void AddDeck(int id, Deck deck);
        void Delete(int id);
        void Delete(string id);
    }
}