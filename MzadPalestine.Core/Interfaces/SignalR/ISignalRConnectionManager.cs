namespace MzadPalestine.Core.Interfaces.SignalR;

public interface ISignalRConnectionManager
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string userId, string connectionId);
    List<string> GetUserConnections(string userId);
    Task SendToUserAsync(string userId, object message, string type = null);
}