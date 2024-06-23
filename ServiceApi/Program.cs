using ServiceApi.Controllers.Hubs;
using ServiceApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
var config = builder.Configuration;

builder.AddServiceDefaults();

service.AddCors();
service.AddServices(config);
service.AddControllers();
service.AddEndpointsApiExplorer();
service.AddSwaggerDocumentation();
service.AddHttpContextAccessor();
service.AddDataProtection();
service.AddMemoryCache();
service.AddJWTAuthentication();
service.AddAuthorization();
service.AddSignalR(o =>{ o.EnableDetailedErrors = true;});
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<MessageHub>("/MessageHub");
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
