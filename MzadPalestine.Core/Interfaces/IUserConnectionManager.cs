using System.Threading.Tasks;

public interface IUserConnectionManager
{
    Task SendToUserAsync(string userId, object message, string type = null);
    void AddConnection(string userId, string connectionId);  //  √ﬂœ „‰ √‰ Â–Â «·œ«·… „ÊÃÊœ…
    void RemoveConnection(string connectionId);
    List<string> GetConnections(string userId);
    public void RemoveConnection(string connectionId, string userId);
   

}