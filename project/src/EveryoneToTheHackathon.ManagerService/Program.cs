using EveryoneToTheHackathon.Infrastructure.BackgroundServices;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.Strategies;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.ManagerService.Consumers;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<ConfigOptions>(configuration);
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfigOptions>>();

builder.Services.AddHostedService<ManagerHostedService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IBackgroundTaskQueue<BaseTaskModel>, BackgroundTaskQueue<BaseTaskModel>>();
builder.Services.AddSingleton<IManagerService, ManagerService>();
builder.Services.AddSingleton<ITeamBuildingStrategy, ManagerTeamBuildingStrategy>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<WishListFormedConsumer>();
    cfg.UsingRabbitMq((context, rcfg) =>
    {
        rcfg.Host("rabbitmq", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rcfg.ReceiveEndpoint("manager-queue", e =>
        {
            e.Consumer<WishListFormedConsumer>(context);
        });
    });
});

var app = builder.Build();

var logRepository = (Hierarchy) LogManager.GetRepository();
XmlConfigurator.Configure(logRepository, new FileInfo(options.Value.Logging!.ConfigFileName));

app.Run();