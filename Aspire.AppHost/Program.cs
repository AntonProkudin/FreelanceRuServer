var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ServiceApi>("apiservice").WithLaunchProfile("https");
var hubService = builder.AddProject<Projects.ServiceHub>("hubservice");
builder.Build().Run();
