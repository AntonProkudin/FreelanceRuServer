using ServiceApi.Database.NativeQuery;
using ServiceApi.Helpers;
using static ServiceApi.Database.EF.ApiDbContext;

namespace ServiceApi.Services.MessageService;

public class MessageService:IMessageService
{
    private readonly UserAvatarNativeSql avatar;
    private readonly MessageNativeSQL db;
    private readonly UserRelationNativeSQL relation;
    private readonly IUserService service;
    public MessageService(IUserService service)
    {
        avatar = new();
        db = new MessageNativeSQL();
        relation = new UserRelationNativeSQL();
        this.service = service;
    }
    public async Task<List<Message>> GetMessages(int userId,int toUserId, DateTime ts) => await db.SelectMessage(userId, toUserId, ts);
    public async Task<int> AddMessage(Message msg) => await db.AddMessage(msg);
    public async Task<List<LastMessages>> GetLastMessages(int fromUserId, DateTime ts) 
    {
        var result = new List<LastMessages>();
        var relationShip = await relation.SelectListUserFriend(fromUserId);
        foreach (var r in relationShip)
        {
            var message = await db.SelectLastMessage(fromUserId, r, ts);
            var user = await service.GetUserInfo(r);
            if (message == null)
            {
                result.Add(new LastMessages()
                {
                    Id = 0,
                    UserId = r,
                    UserInfo = new() { Email = user.Email, Active = user.Active, Avatar = await avatar.GetUrlAvatar(r) != String.Empty ? $"http://{ServerInfo.GetLocalIPAddress()}:5156" + await avatar.GetUrlAvatar(r) : $"http://{ServerInfo.GetLocalIPAddress()}:5156/images/default.png", FirstName = user.FirstName, LastName = user.LastName, Id = user.Id, Ts = user.Ts},
                    IsRead = false,
                    Content = String.Empty,
                    SendDateTime = DateTime.MinValue,

                });
                continue;
            }
            result.Add(new LastMessages()
            {
                Id = message.Id,
                UserId = r,
                UserInfo = new() { Email = user.Email, Active = user.Active, Avatar = await avatar.GetUrlAvatar(r) != String.Empty ? $"http://{ServerInfo.GetLocalIPAddress()}:5156" + await avatar.GetUrlAvatar(r) : $"http://{ServerInfo.GetLocalIPAddress()}:5156/images/default.png", FirstName = user.FirstName, LastName = user.LastName, Id = user.Id, Ts = user.Ts },
                IsRead = message.IsRead,
                Content = message.Text,
                SendDateTime = message.SendTime,

            });
        } 
        return result;
    }
}
