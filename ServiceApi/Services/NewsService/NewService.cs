using Microsoft.EntityFrameworkCore;
using ServiceApi.Models;
using ServiceApi.Models.Responses.News;
using ServiceApi.Models.Responses.Task;
using ServiceApi.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ServiceApi.Database.EF.ApiDbContext;

namespace ServiceApi.Services.TaskService;

public class NewsService : INewsService
{
    private readonly ThisDbContext thisDbContext;
    public NewsService(ThisDbContext thisDbContext)
    {
        this.thisDbContext = thisDbContext;
    }
    public async Task<GetNewResponse> GetNews(int id)
    {
        var news = await thisDbContext.News.Where(x=>x.Id == id).FirstOrDefaultAsync();

        if (news == null)
        {
            return new GetNewResponse
            {
                Success = false,
                Error = "No news found by ID",
                ErrorCode = "T04"
            };
        }
        return new GetNewResponse { Success = true, News = news };
    }
    public async Task<GetNewsResponse> GetAllNews(DateTime ts)
    {
        var news = await thisDbContext.News.Where
                    (o => o.Ts < ts).Take(25).ToListAsync();

        if (news.Count == 0)
        {
            return new GetNewsResponse
            {
                Success = false,
                Error = "No news found",
                ErrorCode = "T04"
            };
        }

        return new GetNewsResponse { Success = true, News = news };
    }
}