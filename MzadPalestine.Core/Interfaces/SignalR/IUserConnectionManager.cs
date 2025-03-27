namespace MzadPalestine.Core.Interfaces.SignalR;

public interface IUserConnectionManager
{
    void KeepUserConnection(string userId, string connectionId);
    void RemoveUserConnection(string connectionId);
    List<string> GetUserConnections(string userId);
    IEnumerable<string> OnlineUsers { get; }
}
