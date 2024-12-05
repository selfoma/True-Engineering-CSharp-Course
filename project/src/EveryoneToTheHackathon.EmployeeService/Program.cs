using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<ConfigOptions>(configuration);
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfigOptions>>();

builder.Services.AddHostedService<EmployeeHostedService>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSingleton<IBackgroundTaskQueue<BaseTaskModel>, BackgroundTaskQueue<BaseTaskModel>>();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddDbContext<HackathonContext>(o =>
{
    o.UseSqlServer(options.Value.Database!.DefaultConnection);
});

var app = builder.Build();
app.MapControllers();

Environment.SetEnvironmentVariable("AppId", Guid.NewGuid().ToString());

var logRepository = (Hierarchy)LogManager.GetRepository();
XmlConfigurator.Configure(logRepository, new FileInfo(options.Value.Logging!.ConfigFileName));

app.Run();