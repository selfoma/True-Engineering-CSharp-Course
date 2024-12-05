using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices.TaskQueues.Models;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<ConfigOptions>(configuration);
var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfigOptions>>();

builder.Services.AddHostedService<DirectorHostedService>();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSingleton<IBackgroundTaskQueue<BaseTaskModel>, BackgroundTaskQueue<BaseTaskModel>>();
builder.Services.AddSingleton<IDirectorService, DirectorService>();
builder.Services.AddSingleton<IHackathonService, HackathonService>();
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

app.MapControllers();

var logRepository = (Hierarchy)LogManager.GetRepository();
XmlConfigurator.Configure(logRepository, new FileInfo(options.Value.Logging!.ConfigFileName));

app.Run();