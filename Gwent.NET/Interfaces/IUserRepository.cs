using Gwent.NET.Model;

namespace Gwent.NET.Interfaces
{
    public interface IUserRepository
    {
        User Find(int id);
        User Create(string name, string picture);
        void Update(int id, User user);
    }
}