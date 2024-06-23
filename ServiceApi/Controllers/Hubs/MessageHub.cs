using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using SignalRSwaggerGen.Attributes;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;
namespace ServiceApi.Controllers.Hubs;

[Authorize]
public partial class MessageHub: Hub
{
    public override async Task OnConnectedAsync()
    {
        HubWrapper.connectionMap.TryAdd(UserID, Context.ConnectionId);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var user in HubWrapper.connectionMap.Where(x => x.Key == UserID))
        {
            HubWrapper.connectionMap.TryRemove(user);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
