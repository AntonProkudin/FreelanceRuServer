using Microsoft.AspNetCore.SignalR;
using ServiceApi.Controllers.Hubs;

namespace ServiceApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MessageController: BaseApiController
{
    private readonly IUserService usrService;
    private readonly IMessageService msgService;
    private readonly IHubContext<MessageHub> hub;
    public MessageController(IServiceProvider provider)
    {
        usrService = provider.GetService<IUserService>();
        msgService = provider.GetService<IMessageService>();
        hub = provider.GetService <IHubContext<MessageHub>>();
    }

    [HttpGet("Relation")]
    public async Task<IActionResult> GetRelation()
    {
        var response = await usrService.GetUserRelation(UserID);
        foreach (var item in response)
        {
            ServiceWrapper.Relation.TryAdd((UserID, item), 1);
        }
        if (response == null)
        {
            return UnprocessableEntity(response);
        }
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> Get(DateTime ts, int toUserId)
    {
        ts = DateTime.Now;
        var response = await msgService.GetMessages(UserID, toUserId, ts);

        if (response == null)
        {
            return UnprocessableEntity(response);
        }
        return Ok(response);
    }

    [HttpGet("Lastest")]
    public async Task<IActionResult> GetLastest(DateTime ts)
    {
        ts = DateTime.Now;
        var response = await msgService.GetLastMessages(UserID, ts);

        if (response == null)
        {
            return UnprocessableEntity(response);
        }
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] Message message)
    {
        if(ServiceWrapper.Relation.Count()<1)
           await GetRelation();
        message.FromUserId = UserID;
        var response = await msgService.AddMessage(message);

        if (response == 0)
        {
            return UnprocessableEntity(response);
        }

        if(HubWrapper.connectionMap.ContainsKey(message.ToUserId))
            await hub.Clients.Clients(HubWrapper.connectionMap[message.ToUserId]).SendAsync("ReceiveMessage", response);

        if (!ServiceWrapper.Relation.ContainsKey((message.FromUserId, message.ToUserId)))
        {
            ServiceWrapper.Relation.TryAdd((message.FromUserId, message.ToUserId), 1);
            await usrService.AddUserRelation(UserID,message.ToUserId);
        }

        return Ok(response);
    }
}
