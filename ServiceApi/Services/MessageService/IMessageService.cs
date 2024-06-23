namespace ServiceApi.Services.MessageService;

public interface IMessageService
{
    Task<List<Message>> GetMessages(int userId, int toUserId, DateTime ts);
    Task<List<LastMessages>> GetLastMessages(int fromUserId, DateTime ts);
    Task<int> AddMessage(Message msg);
}
