namespace ServiceApi.Models.Responses.Authorization;

public class TokenResponse : BaseResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }

}