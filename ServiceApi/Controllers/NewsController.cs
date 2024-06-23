using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceApi.Services.TaskService;

namespace ServiceApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NewsController:BaseApiController
{
    private readonly INewsService newsService;
    public NewsController(IServiceProvider provider)
    {
        newsService = provider.GetService<INewsService>();
    }
    [HttpGet]
    public async Task<IActionResult> Get(DateTime ts)
    {
        var response = await newsService.GetAllNews(ts);

        if (!response.Success)
        {
            return UnprocessableEntity(response);
        }

        var newsResponse = response?.News?.ConvertAll(o =>
        new NewsResponse
        {
            Id = o.Id,
            Title = o.Title,
            Description = o.Description,
            Url = o.Url,
            Ts = o.Ts
        });

        return Ok(newsResponse);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await newsService.GetNews(id);

        if (!response.Success)
        {
            return UnprocessableEntity(response);
        }

        return Ok(response);
    }

}
