using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace twitchDnd.Server.Hubs;

[Authorize]
public class ServerSignalRHub : Hub
{
}