using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MzadPalestine.Core.Interfaces.Services;

namespace MzadPalestine.Infrastructure.SignalR;

[Authorize]
public class NotificationHub : Hub
{
    private readonly IUserConnectionManager _userConnectionManager;
    private readonly ICurrentUserService _currentUserService;

    public NotificationHub(
        IUserConnectionManager userConnectionManager,
        ICurrentUserService currentUserService)
    {
        _userConnectionManager = userConnectionManager;
        _currentUserService = currentUserService;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = _currentUserService.UserId;
        var connectionId = Context.ConnectionId;

        _userConnectionManager.AddConnection(userId!, connectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = _currentUserService.UserId;
        var connectionId = Context.ConnectionId;

        _userConnectionManager.RemoveConnection(userId!, connectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
