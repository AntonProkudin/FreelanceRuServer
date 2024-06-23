using ServiceApi.Models;
using ServiceApi.Models.Responses.Task;
using ServiceApi.Models.Responses.User;

namespace ServiceApi.Services.SkillsService;

public interface ISkillService
{
    Task<GetSkillsResponse> GetSkills(int userId);
    Task<SaveSkillResponse> SaveSkill(UserSkill skill);
    Task<DeleteSkillResponse> DeleteSkill(int skillId, int userId);
}
