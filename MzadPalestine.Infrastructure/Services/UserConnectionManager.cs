using System.Collections.Concurrent;
using MzadPalestine.Core.Interfaces.Services;

namespace MzadPalestine.Infrastructure.Services;

public class UserConnectionManager : IUserConnectionManager
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _userConnections;

    public UserConnectionManager()
    {
        _userConnections = new ConcurrentDictionary<string, HashSet<string>>();
    }

    public void AddConnection(string userId, string connectionId)
    {
        _userConnections.AddOrUpdate(
            userId,
            new HashSet<string> { connectionId },
            (_, connections) =>
            {
                connections.Add(connectionId);
                return connections;
            });
    }

    public void RemoveConnection(string connectionId)
    {
        var userToRemove = _userConnections
            .FirstOrDefault(x => x.Value.Contains(connectionId));

        if (userToRemove.Key != null)
        {
            _userConnections.TryGetValue(userToRemove.Key, out var connections);
            if (connections != null)
            {
                connections.Remove(connectionId);
                if (!connections.Any())
                {
                    _userConnections.TryRemove(userToRemove.Key, out _);
                }
            }
        }
    }

    public IEnumerable<string> GetConnections(string userId)
    {
        return _userConnections.TryGetValue(userId, out var connections)
            ? connections
            : Enumerable.Empty<string>();
    }

    public bool HasConnections(string userId)
    {
        return _userConnections.TryGetValue(userId, out var connections) && connections.Any();
    }

    public Task SendToUserAsync(string userId, object message, string type = null)
    {
        throw new NotImplementedException();
    }

    List<string> IUserConnectionManager.GetConnections(string userId)
    {
        throw new NotImplementedException();
    }

    public void RemoveConnection(string connectionId, string userId)
    {
        throw new NotImplementedException();
    }
}
