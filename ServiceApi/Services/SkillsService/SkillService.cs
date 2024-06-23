using ServiceApi.Models;
using ServiceApi.Models.Responses.Task;
using ServiceApi.Models.Responses.User;
using static ServiceApi.Database.EF.ApiDbContext;

namespace ServiceApi.Services.SkillsService;

public class SkillService : ISkillService
{
    private readonly ThisDbContext thisDbContext;
    public SkillService(ThisDbContext thisDbContext)
    {
        this.thisDbContext = thisDbContext;
    }

    public async Task<DeleteSkillResponse> DeleteSkill(int skillId, int userId)
    {
        var skill = await thisDbContext.UserSkills.FindAsync(skillId);

        if (skill == null)
        {
            return new DeleteSkillResponse
            {
                Success = false,
                Error = "Skill not found",
                ErrorCode = "T01"
            };
        }

        if (skill.UserId != userId)
        {
            return new DeleteSkillResponse
            {
                Success = false,
                Error = "You don't have access to delete this skill",
                ErrorCode = "T02"
            };
        }

        thisDbContext.UserSkills.Remove(skill);
        var saveResponse = await thisDbContext.SaveChangesAsync();

        if (saveResponse >= 0)
        {
            return new DeleteSkillResponse
            {
                Success = true,
                SkillId = skill.Id
            };
        }

        return new DeleteSkillResponse
        {
            Success = false,
            Error = "Unable to delete skill",
            ErrorCode = "T03"
        };
    }
    public async Task<GetSkillsResponse> GetSkills(int userId)
    {
        var skills = await thisDbContext.UserSkills.Where
                    (o => o.UserId == userId).ToListAsync();

        if (skills.Count == 0)
        {
            return new GetSkillsResponse
            {
                Success = false,
                Error = "No skills found for this user",
                ErrorCode = "T04"
            };
        }

        return new GetSkillsResponse { Success = true, Skills = skills };
    }
    public async Task<SaveSkillResponse> SaveSkill(UserSkill skill)
    {
        await thisDbContext.UserSkills.AddAsync(skill);

        var saveResponse = await thisDbContext.SaveChangesAsync();

        if (saveResponse >= 0)
        {
            return new SaveSkillResponse
            {
                Success = true,
                Skill = skill
            };
        }
        return new SaveSkillResponse
        {
            Success = false,
            Error = "Unable to save skill",
            ErrorCode = "T05"
        };
    }
}