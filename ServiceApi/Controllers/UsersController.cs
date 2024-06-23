using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceApi.Helpers;
using ServiceApi.Models;
#pragma warning disable CS8602
#pragma warning disable CS8601
#pragma warning disable CS8618
namespace ServiceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseApiController
{
    private readonly IUserService userService;
    private readonly ITokenService tokenService;
    private readonly ISkillService skillService;
    public UsersController(IServiceProvider provider)
    {
        userService = provider.GetService<IUserService>();
        tokenService = provider.GetService<ITokenService>();
        skillService = provider.GetService<ISkillService>();
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) ||
            string.IsNullOrEmpty(loginRequest.Password))
        {
            return BadRequest(new TokenResponse
            {
                Error = "Missing login details",
                ErrorCode = "L01"
            });
        }

        var loginResponse = await userService.LoginAsync(loginRequest);

        if (!loginResponse.Success)
        {
            return Unauthorized(new
            {
                loginResponse.ErrorCode,
                loginResponse.Error
            });
        }

        return Ok(loginResponse);
    }

    [HttpPost]
    [Route("Refresh_Token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        if (refreshTokenRequest == null || string.IsNullOrEmpty
        (refreshTokenRequest.RefreshToken) || refreshTokenRequest.UserId == 0)
        {
            return BadRequest(new TokenResponse
            {
                Error = "Missing refresh token details",
                ErrorCode = "R01"
            });
        }

        var validateRefreshTokenResponse =
            await tokenService.ValidateRefreshTokenAsync(refreshTokenRequest);

        if (!validateRefreshTokenResponse.Success)
        {
            return UnprocessableEntity(validateRefreshTokenResponse);
        }

        var tokenResponse = await tokenService.GenerateTokensAsync
                            (validateRefreshTokenResponse.UserId);

        return Ok(new
        {
            AccessToken = tokenResponse.Item1,
            Refreshtoken = tokenResponse.Item2
        });
    }

    [HttpPost]
    [Route("Signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest signupRequest)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany
                         (x => x.Errors.Select(c => c.ErrorMessage)).ToList();
            if (errors.Any())
            {
                return BadRequest(new TokenResponse
                {
                    Error = $"{string.Join(",", errors)}",
                    ErrorCode = "S01"
                });
            }
        }

        var signupResponse = await userService.SignupAsync(signupRequest);

        if (!signupResponse.Success)
        {
            return UnprocessableEntity(signupResponse);
        }

        return Ok(signupResponse.Email);
    }

    [Authorize]
    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        var logout = await userService.LogoutAsync(UserID);

        if (!logout.Success)
        {
            return UnprocessableEntity(logout);
        }

        return Ok();
    }

    [Authorize]
    [HttpDelete("UserSkill/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteSkillResponse = await skillService.DeleteSkill(id, UserID);
        if (!deleteSkillResponse.Success)
        {
            return UnprocessableEntity(deleteSkillResponse);
        }

        return Ok(deleteSkillResponse.SkillId);
    }

    [Authorize]
    [HttpPost("UserSkill")]
    public async Task<IActionResult> Post(SkillRequest request)
    {
        var skill = new UserSkill
        {
            Title = request.Title,
            UserId = UserID
        };

        var saveSkillResponse = await skillService.SaveSkill(skill);

        if (!saveSkillResponse.Success)
        {
            return UnprocessableEntity(saveSkillResponse);
        }

        var skillResponse = new SkillResponse
        {
            Id = saveSkillResponse.Skill.Id,
            Title = saveSkillResponse.Skill.Title,
            UserId = UserID,
        };

        return Ok(skillResponse);
    }

    [Authorize]
    [HttpGet("UserSkill/{id}")]
    public async Task<IActionResult> GetUserSkills(int id)
    {
        var getSkillsResponse = await skillService.GetSkills(id);

        if (!getSkillsResponse.Success)
        {
            return UnprocessableEntity(getSkillsResponse);
        }

        var tasksResponse = getSkillsResponse?.Skills?.ConvertAll(o =>
        new SkillResponse
        {
            Id = o.Id,
            UserId = o.UserId,
            Title = o.Title,
        });

        return Ok(tasksResponse);
    }
    [Authorize]
    [HttpGet("UserInfo")]
    public async Task<IActionResult> Get()
    {
        int id = UserID;
        var user = await userService.GetUserInfo(id);

        if (user == null)
        {
            return UnprocessableEntity(user);
        }
        var getSkillsResponse = await skillService.GetSkills(id);
        var tasksResponse = getSkillsResponse?.Skills?.ConvertAll(o =>
        new SkillsResponse
        {
            Id = o.Id,
            Title = o.Title,
        });
        var getUserAvatarResponse = await userService.GetUserAvatar(id);
        var response = new UserInfoResponse()
        {
            Id = UserID,
            Active = user.Active,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Avatar = getUserAvatarResponse != String.Empty ? $"http://{ServerInfo.GetLocalIPAddress()}:5156" + getUserAvatarResponse : $"http://{ServerInfo.GetLocalIPAddress()}:5156/images/default.png",
            Ts = user.Ts,
            UserSkills = tasksResponse,
        };
        return Ok(response);
    }
    [Authorize]
    [HttpGet("UserInfo/{userId}")]
    public async Task<IActionResult> GetById(int userId)
    {
        int id = userId;
        var user = await userService.GetUserInfo(id);

        if (user == null)
        {
            return UnprocessableEntity(user);
        }
        var getSkillsResponse = await skillService.GetSkills(id);
        var tasksResponse = getSkillsResponse?.Skills?.ConvertAll(o =>
        new SkillsResponse
        {
            Id = o.Id,
            Title = o.Title,
        });
        var getUserAvatarResponse = await userService.GetUserAvatar(id);
        var response = new UserInfoResponse()
        {
            Id = userId,
            Active = user.Active,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Avatar = getUserAvatarResponse != String.Empty ? $"http://{ServerInfo.GetLocalIPAddress()}:5156" + getUserAvatarResponse : $"http://{ServerInfo.GetLocalIPAddress()}:5156/images/default.png",
            Ts = user.Ts,
            UserSkills = tasksResponse,
        };
        return Ok(response);
    }

    [Authorize]
    [HttpPost("SetUserAvatar")]
    public async Task<IActionResult> SetAvatar(SetUserAvatarRequest req)
    {
        int id = UserID;
        var isCreated = await userService.SetUserAvatar(id, req.Url);
        if (isCreated)
            return Ok();
        else
            return BadRequest();
    }
}