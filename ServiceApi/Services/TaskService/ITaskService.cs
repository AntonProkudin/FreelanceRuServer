using ServiceApi.Models;
using ServiceApi.Models.Responses.Task;

namespace ServiceApi.Services.TaskService;

public interface ITaskService
{
    Task<GetTasksResponse> GetTasks(int userId);
    Task<GetTasksResponse> GetAllTasks(DateTime ts, int userId);
    Task<SaveTaskResponse> SaveTask(Models.Task task);
    Task<DeleteTaskResponse> DeleteTask(int taskId, int userId);
}