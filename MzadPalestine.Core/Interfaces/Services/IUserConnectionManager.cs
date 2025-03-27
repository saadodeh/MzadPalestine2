namespace MzadPalestine.Core.Interfaces.Services;

public interface IUserConnectionManager
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string connectionId);
    IEnumerable<string> GetConnections(string userId);
    bool HasConnections(string userId);
}
