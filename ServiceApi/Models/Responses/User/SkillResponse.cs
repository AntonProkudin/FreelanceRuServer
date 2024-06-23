using System.Text.Json.Serialization;

namespace ServiceApi.Models.Responses.User;

public class SaveSkillResponse : BaseResponse
{
    public UserSkill? Skill { get; set; }
}
public class GetSkillsResponse : BaseResponse
{
    public List<UserSkill>? Skills { get; set; }
}
public class DeleteSkillResponse : BaseResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int SkillId { get; set; }
}
public class SkillResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required int UserId { get; set; }
}
public class SkillsResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
}