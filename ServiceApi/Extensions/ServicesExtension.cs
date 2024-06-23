using ServiceApi.Services.MessageService;
using static ServiceApi.Database.EF.ApiDbContext;

namespace ServiceApi.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection service, IConfiguration config)
    {

        service.AddDbContext<ThisDbContext>(options => options.UseSqlServer(config["ConnectionStrings:conn"]));
        service.AddTransient<ITokenService, TokenService>();
        service.AddTransient<INewsService, NewsService>();
        service.AddTransient<IUserService, UserService>();
        service.AddTransient<ITaskService, TaskService>();
        service.AddTransient<ICategoryService, CategoryService>();
        service.AddTransient<ISkillService, SkillService>();
        service.AddTransient<IHelpService, HelpService>();
        service.AddTransient<IMessageService, MessageService>();
        return service;
    }
}
