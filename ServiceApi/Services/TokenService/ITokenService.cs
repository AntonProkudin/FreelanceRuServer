using ServiceApi.Models.Requests.Authorization;
using ServiceApi.Models.Responses.Authorization;
using ServiceApi.Models;

namespace ServiceApi.Services.TokenService;

public interface ITokenService
{
    Task<Tuple<string, string>> GenerateTokensAsync(int userId);
    Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync
                                       (RefreshTokenRequest refreshTokenRequest);
    Task<bool> RemoveRefreshTokenAsync(User user);
}
