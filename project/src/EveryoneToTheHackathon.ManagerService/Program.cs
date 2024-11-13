using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.Strategies;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<ConfigOptions>(configuration);
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfigOptions>>();

builder.Services.AddHostedService<HostedService>();
builder.Services.AddSingleton<IDirectorService, DirectorService>();
builder.Services.AddSingleton<IManagerService, ManagerService>();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IHackathonService, HackathonService>();
builder.Services.AddSingleton<ITeamBuildingStrategy, ManagerTeamBuildingStrategy>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IHackathonRepository, HackathonRepository>();

builder.Services.AddDbContext<HackathonContext>(o =>
{
    o.UseSqlServer(options.Value.Database!.DefaultConnection);
});

var app = builder.Build();
using var scope = app.Services.CreateScope();
{
    var context = scope.ServiceProvider.GetRequiredService<HackathonContext>();
    context.Database.Migrate();
}

var logRepository = (Hierarchy)LogManager.GetRepository();
XmlConfigurator.Configure(logRepository, new FileInfo(options.Value.Logging!.ConfigFileName));

app.Run();