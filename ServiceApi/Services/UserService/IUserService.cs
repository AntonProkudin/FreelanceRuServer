using ServiceApi.Models.Requests.Authorization;
using ServiceApi.Models.Responses.Authorization;

namespace ServiceApi.Services.UserService;

public interface IUserService
{
    Task<TokenResponse> LoginAsync(LoginRequest loginRequest);
    Task<SignupResponse> SignupAsync(SignupRequest signupRequest);
    Task<LogoutResponse> LogoutAsync(int userId);
    Task<User> GetUserInfo(int userId);
    Task<string> GetUserAvatar(int userId);
    Task<bool> SetUserAvatar(int userId, string url);
    Task<List<int>> GetUserRelation(int userId);
    Task<int> AddUserRelation(int userId, int to);
}