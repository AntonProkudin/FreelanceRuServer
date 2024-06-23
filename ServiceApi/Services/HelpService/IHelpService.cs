namespace ServiceApi.Services.HelpService;

public interface IHelpService
{
    Task<SendImageResponse> LoadImage(IFormFile file);
}
