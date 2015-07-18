using System.Collections.Generic;

namespace Gwent.NET.Interfaces
{
    public interface IUserConnectionMap
    {
        void Connect(string connectionId, string userId);

        void Disconnect(string connectionId);

        IEnumerable<string> GetConnections();

        IEnumerable<string> GetUsers();

        IEnumerable<string> GetConnections(string userId);

        string GetUser(string connectionId);
    }
}
