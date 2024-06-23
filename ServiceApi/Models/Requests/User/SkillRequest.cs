namespace ServiceApi.Models.Requests.User;

public class SkillRequest
{
    public required string Title { get; set; }
}
public class SetUserAvatarRequest
{
    public required string Url { get; set; }
}