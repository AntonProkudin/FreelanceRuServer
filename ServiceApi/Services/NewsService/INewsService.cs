using ServiceApi.Models;

using ServiceApi.Models.Responses.Task;

namespace ServiceApi.Services.TaskService;

public interface INewsService
{
    Task<GetNewResponse> GetNews(int userId);
    Task<GetNewsResponse> GetAllNews(DateTime ts);
}