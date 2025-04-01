using System.Collections.Concurrent;
using MzadPalestine.Core.Interfaces.SignalR;

namespace MzadPalestine.Infrastructure.SignalR;

public class UserConnectionManager : ISignalRConnectionManager
{
    private readonly ConcurrentDictionary<string, List<string>> _userConnectionMap = new();

    public void AddConnection(string userId, string connectionId)
    {
        if (!_userConnectionMap.ContainsKey(userId))
        {
            _userConnectionMap[userId] = new List<string>();
        }

        _userConnectionMap[userId].Add(connectionId);
    }

    public void RemoveConnection(string userId, string connectionId)
    {
        if (_userConnectionMap.TryGetValue(userId, out var connections))
        {
            connections.Remove(connectionId);
            if (connections.Count == 0)
            {
                _userConnectionMap.TryRemove(userId, out _);
            }
        }
    }

    public List<string> GetUserConnections(string userId)
    {
        return _userConnectionMap.GetValueOrDefault(userId) ?? new List<string>();
    }

    public Task SendToUserAsync(string userId, object message, string type = null)
    {
        throw new NotImplementedException();
    }

    public List<string> GetConnections(string userId)
    {
        return GetUserConnections(userId);
    }

    public IEnumerable<string> OnlineUsers => _userConnectionMap.Keys;
}
