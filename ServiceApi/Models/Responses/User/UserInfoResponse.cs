namespace ServiceApi.Models.Responses.User;

public class UserInfoResponse
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Avatar { get; set; }
    public required DateTime Ts { get; set; }
    public required bool Active { get; set; }
    public virtual ICollection<SkillsResponse> UserSkills { get; set; }
}
