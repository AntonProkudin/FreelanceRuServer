namespace ServiceApi.Models;

public class UserRelation
{
    public int UserId { get; set; }
    public Message LastMessage { get; set; }
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }
}
