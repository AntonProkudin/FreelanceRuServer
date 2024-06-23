namespace ServiceApi.Models;

public class LastMessages
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public UserInfoResponse UserInfo { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime SendDateTime { get; set; }
    public bool IsRead { get; set; }
}
