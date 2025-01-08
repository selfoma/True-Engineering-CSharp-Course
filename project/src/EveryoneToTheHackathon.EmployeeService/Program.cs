using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.EmployeeService.Consumers;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using MassTransit;
using MassTransit.Transports.Fabric;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<ConfigOptions>(configuration);
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfigOptions>>();

builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();

var role = EmployeeRoleExtensions.GetRole(Environment.GetEnvironmentVariable("role"));
var id = Convert.ToInt32(Environment.GetEnvironmentVariable("id"));

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<HackathonStartedConsumer>();
    cfg.UsingRabbitMq((context, rcfg) =>
    {
        rcfg.Host("rabbitmq", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        rcfg.ReceiveEndpoint($"employee-{role}-{id}", e =>
        {
            e.Consumer<HackathonStartedConsumer>(context);
        });
    });
});

builder.Services.AddDbContext<HackathonContext>(o =>
{
    o.UseSqlServer(options.Value.Database!.DefaultConnection);
});

var app = builder.Build();

Environment.SetEnvironmentVariable("AppId", $"employee-{role}-{id}");

var logRepository = (Hierarchy)LogManager.GetRepository();
XmlConfigurator.Configure(logRepository, new FileInfo(options.Value.Logging!.ConfigFileName));

app.Run();