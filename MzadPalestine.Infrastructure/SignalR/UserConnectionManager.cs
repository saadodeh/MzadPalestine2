using System.Collections.Concurrent;
using MzadPalestine.Core.Interfaces.Services;

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

        if (!_userConnectionMap[userId].Contains(connectionId))
        {
            _userConnectionMap[userId].Add(connectionId);
        }
    }

    public void RemoveUserConnection(string connectionId)
    {
        foreach (var kvp in _userConnectionMap)
        {
            if (kvp.Value.Contains(connectionId))
            {
                kvp.Value.Remove(connectionId);
                if (kvp.Value.Count == 0)
                {
                    _userConnectionMap.TryRemove(kvp.Key, out _);
                }
                break;
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
