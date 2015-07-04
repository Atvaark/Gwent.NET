using System.Collections.Generic;

namespace Gwent.NET.Interfaces
{
    public interface IUserConnectionMap
    {
        void Connect(string connectionId);
        
        void Disconnect(string connectionId);

        void Authenticate(string connectionId, string userId);
        IEnumerable<string> GetConnections();
        IEnumerable<string> GetUsers();
        IEnumerable<string> GetConnections(string userId);
        string GetUser(string connectionId);
    }
}
