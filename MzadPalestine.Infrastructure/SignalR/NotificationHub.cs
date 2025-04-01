using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Interfaces.SignalR;
namespace MzadPalestine.Infrastructure.SignalR;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ISignalRConnectionManager _userConnectionManager;
    private readonly Core.Interfaces.Services.ICurrentUserService _currentUserService;

    public NotificationHub(
        ISignalRConnectionManager userConnectionManager,
        Core.Interfaces.Services.ICurrentUserService currentUserService)
    {
        _userConnectionManager = userConnectionManager;
        _currentUserService = currentUserService;
    }

    public override async Task OnConnectedAsync( )
    {
        var userId = _currentUserService.UserId;
        var connectionId = Context.ConnectionId;

        _userConnectionManager.AddConnection(userId! , connectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = _currentUserService.UserId;
        var connectionId = Context.ConnectionId;

        _userConnectionManager.RemoveConnection(userId! , connectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
