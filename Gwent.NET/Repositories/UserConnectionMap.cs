using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gwent.NET.Extensions;
using Gwent.NET.Interfaces;

namespace Gwent.NET.Repositories
{
    public class UserConnectionMap : IUserConnectionMap
    {
        private readonly ReaderWriterLockSlim _readerWriterLock;
        private readonly Dictionary<string, HashSet<string>> _userToConnectionsMap;
        private readonly Dictionary<string, string> _connectionToUserMap;
        private const string DefaultUserId = null;

        public UserConnectionMap()
        {
            _readerWriterLock = new ReaderWriterLockSlim();
            _userToConnectionsMap = new Dictionary<string, HashSet<string>>();
            _connectionToUserMap = new Dictionary<string, string>();
        }

        public void Connect(string connectionId)
        {
            using (_readerWriterLock.EnterWrite())
            {
                string userId;
                if (_connectionToUserMap.TryGetValue(connectionId, out userId) && userId != DefaultUserId)
                {
                    HashSet<string> connectionIds = _userToConnectionsMap[userId];
                    connectionIds.Remove(connectionId);
                }
                else
                {
                    _connectionToUserMap[connectionId] = DefaultUserId;
                } 
            }
        }
        
        public void Disconnect(string connectionId)
        {
            using (_readerWriterLock.EnterWrite())
            {
                string userId;
                if (_connectionToUserMap.TryGetValue(connectionId, out userId))
                {
                    HashSet<string> connectionIds = _userToConnectionsMap[userId];
                    connectionIds.Remove(connectionId);
                }
                _connectionToUserMap.Remove(connectionId);
            }
        }

        public void Authenticate(string connectionId, string userId)
        {
            using (_readerWriterLock.EnterWrite())
            {
                HashSet<string> connectionIds;
                if (!_userToConnectionsMap.TryGetValue(userId, out connectionIds))
                {
                    connectionIds = new HashSet<string>();
                    _userToConnectionsMap[userId] = connectionIds;
                } 
                connectionIds.Add(connectionId);
                _connectionToUserMap[connectionId] = userId;
            }
        }
        
        public IEnumerable<string> GetConnections()
        {
            using (_readerWriterLock.EnterWrite())
            {
                return _connectionToUserMap.Keys.ToList();
            }
        }
        public IEnumerable<string> GetUsers()
        {
            using (_readerWriterLock.EnterWrite())
            {
                return _userToConnectionsMap.Keys.ToList();
            }
        }
        public IEnumerable<string> GetConnections(string userId)
        {
            using (_readerWriterLock.EnterRead())
            {
                HashSet<string> connectionIds;
                if (_userToConnectionsMap.TryGetValue(userId, out connectionIds))
                {
                    return connectionIds.ToList();
                }
                return Enumerable.Empty<string>();
            }
        }

        public string GetUser(string connectionId)
        {
            using (_readerWriterLock.EnterWrite())
            {
                string userId;
                _connectionToUserMap.TryGetValue(connectionId, out userId);
                return userId;
            }
        }
    }
}