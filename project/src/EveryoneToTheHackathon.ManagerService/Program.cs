using EveryoneToTheHackathon.Infrastructure.BackgroundServices;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.Strategies;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<ConfigOptions>(configuration);
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfigOptions>>();

builder.Services.AddHostedService<ManagerHostedService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IBackgroundTaskQueue<BaseTaskModel>, BackgroundTaskQueue<BaseTaskModel>>();
builder.Services.AddControllers();
builder.Services.AddSingleton<IManagerService, ManagerService>();
builder.Services.AddSingleton<ITeamBuildingStrategy, ManagerTeamBuildingStrategy>();

var app = builder.Build();

var logRepository = (Hierarchy)LogManager.GetRepository();
XmlConfigurator.Configure(logRepository, new FileInfo(options.Value.Logging!.ConfigFileName));

app.Run();