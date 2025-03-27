using System.Collections.Concurrent;
using MzadPalestine.Core.Interfaces.SignalR;

namespace MzadPalestine.Infrastructure.SignalR;

public class UserConnectionManager : IUserConnectionManager
{
    private readonly ConcurrentDictionary<string, List<string>> _userConnectionMap = new();

    public void KeepUserConnection(string userId, string connectionId)
    {
        if (!_userConnectionMap.ContainsKey(userId))
        {
            _userConnectionMap[userId] = new List<string>();
        }

        _userConnectionMap[userId].Add(connectionId);
    }

    public void RemoveUserConnection(string connectionId)
    {
        foreach (var userId in _userConnectionMap.Keys)
        {
            if (_userConnectionMap.TryGetValue(userId, out var connections))
            {
                if (connections.Contains(connectionId))
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _userConnectionMap.TryRemove(userId, out _);
                    }
                    break;
                }
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

    public void AddConnection(string userId, string connectionId)
    {
        throw new NotImplementedException();
    }

    public void RemoveConnection(string connectionId)
    {
        throw new NotImplementedException();
    }

    public List<string> GetConnections(string userId)
    {
        throw new NotImplementedException();
    }

    public void RemoveConnection(string connectionId, string userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> OnlineUsers => _userConnectionMap.Keys;
}
