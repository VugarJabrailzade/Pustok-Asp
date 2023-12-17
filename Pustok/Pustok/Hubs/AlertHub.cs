using Microsoft.AspNetCore.SignalR;
using Pustok.Services.Abstract;
using System;
using System.Threading.Tasks;

namespace Pustok.Hubs;

public class AlertHub : Hub
{
    private readonly IUserService _userService;
    private readonly IHubContext<UsersPageHub> _userPageHubContext;

    public AlertHub(IUserService userService, IHubContext<UsersPageHub> userPageHubContext)
    {
        _userService = userService;
        _userPageHubContext = userPageHubContext;
    }

    public override Task OnConnectedAsync()
    {
        _userService.AddCurrentUserConnection(Context.ConnectionId);

        _userPageHubContext.Clients.All.SendAsync("ReceiveUserStatus", 
                            new {UserId = _userService.CurrentUser.Id, IsOnline = true});

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _userService.RemoveCurrentUserConnection(Context.ConnectionId);

        _userPageHubContext.Clients.All.SendAsync("ReceiveUserStatus",
                           new { UserId = _userService.CurrentUser.Id, IsOnline = false });

        return base.OnDisconnectedAsync(exception);
    }

    
}
