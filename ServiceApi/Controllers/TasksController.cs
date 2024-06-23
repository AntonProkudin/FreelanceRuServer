using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS8602
#pragma warning disable CS8601
#pragma warning disable CS8618
namespace ServiceApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TasksController : BaseApiController
{
    private readonly ITaskService taskService;
    private readonly ICategoryService categoryService;
    public TasksController(IServiceProvider provider)
    {
        taskService = provider.GetService<ITaskService>();
        categoryService = provider.GetService<ICategoryService>();
    }
    [HttpGet]
    public async Task<IActionResult> Get(DateTime ts)
    {
        var getTasksResponse = await taskService.GetAllTasks(ts, UserID);

        if (!getTasksResponse.Success)
        {
            return UnprocessableEntity(getTasksResponse);
        }

        var tasksResponse = getTasksResponse?.Tasks?.ConvertAll(o =>
        new TaskResponse
        {
            UserId = o.UserId,
            Category = ServiceWrapper.CategoryList[o.CategoryId],
            Price = o.Price,
            Description = o.Description,
            Id = o.Id,
            IsCompleted = o.IsCompleted,
            Title = o.Title,
            Ts = o.Ts
        });

        return Ok(tasksResponse);
    }
    [HttpGet("ThisUser")]
    public async Task<IActionResult> Get()
    {
        var getTasksResponse = await taskService.GetTasks(UserID);

        if (!getTasksResponse.Success)
        {
            return UnprocessableEntity(getTasksResponse);
        }

        var tasksResponse = getTasksResponse?.Tasks?.ConvertAll(o =>
        new TaskResponse
        {
            UserId = o.UserId,
            Category = ServiceWrapper.CategoryList[o.CategoryId],
            Price = o.Price,
            Description = o.Description,
            Id = o.Id,
            IsCompleted = o.IsCompleted,
            Title = o.Title,
            Ts = o.Ts
        });

        return Ok(tasksResponse);
    }
    [HttpGet("Category")]
    public async Task<IActionResult> GetCategories()
    {
        var getCategoriesResponse = await categoryService.GetCategories();

        if (!getCategoriesResponse.Success)
        {
            return UnprocessableEntity(getCategoriesResponse);
        }

        var tasksResponse = getCategoriesResponse?.Categories?.ConvertAll(o =>
        new CategoryResponse
        {
            Id = o.Id,
            Title = o.Title
        });

        return Ok(tasksResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(TaskRequest taskRequest)
    {
        var task = new Models.Task
        {
            Price = taskRequest.Price,
            Description = taskRequest.Description,
            CategoryId = taskRequest.CategoryId,
            IsCompleted = taskRequest.IsCompleted,
            Ts = DateTime.Now,
            Title = taskRequest.Title,

            UserId = UserID
        };

        var saveTaskResponse = await taskService.SaveTask(task);

        if (!saveTaskResponse.Success)
        {
            return UnprocessableEntity(saveTaskResponse);
        }

        var taskResponse = new TaskResponse
        {
            UserId = UserID,
            Category = ServiceWrapper.CategoryList[saveTaskResponse.Task.CategoryId],
            Price = saveTaskResponse.Task.Price,
            Description = saveTaskResponse.Task.Description,
            Id = saveTaskResponse.Task.Id,
            IsCompleted = saveTaskResponse.Task.IsCompleted,
            Title = saveTaskResponse.Task.Title,
            Ts = saveTaskResponse.Task.Ts
        };

        return Ok(taskResponse);
    }

    [HttpPost("Category")]
    public async Task<IActionResult> PostCategory(CategoryRequest request)
    {
        var category = new Category
        {
            Title = request.Title,
        };

        var saveCategoryResponse = await categoryService.SaveCategory(category);

        if (!saveCategoryResponse.Success)
        {
            return UnprocessableEntity(saveCategoryResponse);
        }

        var categoryResponse = new CategoryResponse
        {
            Id = saveCategoryResponse.Category.Id,
            Title = saveCategoryResponse.Category.Title,
        };

        return Ok(categoryResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleteTaskResponse = await taskService.DeleteTask(id, UserID);
        if (!deleteTaskResponse.Success)
        {
            return UnprocessableEntity(deleteTaskResponse);
        }

        return Ok(deleteTaskResponse.TaskId);
    }

    [HttpDelete("Category/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleteCategoryResponse = await categoryService.DeleteCategory(id);
        if (!deleteCategoryResponse.Success)
        {
            return UnprocessableEntity(deleteCategoryResponse);
        }

        return Ok(deleteCategoryResponse.CategoryId);
    }
}