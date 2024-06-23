using Microsoft.EntityFrameworkCore;
using ServiceApi.Models;
using ServiceApi.Models.Responses.Task;
using ServiceApi.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ServiceApi.Database.EF.ApiDbContext;

namespace ServiceApi.Services.TaskService;

public class TaskService : ITaskService
{
    private readonly ThisDbContext thisDbContext;
    public TaskService(ThisDbContext thisDbContext)
    {
        this.thisDbContext = thisDbContext;
    }

    public async Task<DeleteTaskResponse> DeleteTask(int taskId, int userId)
    {
        var task = await thisDbContext.Tasks.FindAsync(taskId);

        if (task == null)
        {
            return new DeleteTaskResponse
            {
                Success = false,
                Error = "Task not found",
                ErrorCode = "T01"
            };
        }

        if (task.UserId != userId)
        {
            return new DeleteTaskResponse
            {
                Success = false,
                Error = "You don't have access to delete this task",
                ErrorCode = "T02"
            };
        }

        thisDbContext.Tasks.Remove(task);
        var saveResponse = await thisDbContext.SaveChangesAsync();

        if (saveResponse >= 0)
        {
            return new DeleteTaskResponse
            {
                Success = true,
                TaskId = task.Id
            };
        }

        return new DeleteTaskResponse
        {
            Success = false,
            Error = "Unable to delete task",
            ErrorCode = "T03"
        };
    }
    public async Task<GetTasksResponse> GetTasks(int userId)
    {
        var tasks = await thisDbContext.Tasks.Where
                    (o => o.UserId == userId).ToListAsync();

        if (tasks.Count == 0)
        {
            return new GetTasksResponse
            {
                Success = false,
                Error = "No tasks found for this user",
                ErrorCode = "T04"
            };
        }

        return new GetTasksResponse { Success = true, Tasks = tasks };
    }
    public async Task<GetTasksResponse> GetAllTasks(DateTime ts, int userId)
    {
        var tasks = await thisDbContext.Tasks.Where
                    (o => o.Ts < ts && o.UserId!= userId).Take(25).ToListAsync();

        if (tasks.Count == 0)
        {
            return new GetTasksResponse
            {
                Success = false,
                Error = "No tasks found",
                ErrorCode = "T04"
            };
        }

        return new GetTasksResponse { Success = true, Tasks = tasks };
    }
    public async Task<SaveTaskResponse> SaveTask(Models.Task task)
    {
        await thisDbContext.Tasks.AddAsync(task);

        var saveResponse = await thisDbContext.SaveChangesAsync();

        if (saveResponse >= 0)
        {
            return new SaveTaskResponse
            {
                Success = true,
                Task = task
            };
        }
        return new SaveTaskResponse
        {
            Success = false,
            Error = "Unable to save task",
            ErrorCode = "T05"
        };
    }
}