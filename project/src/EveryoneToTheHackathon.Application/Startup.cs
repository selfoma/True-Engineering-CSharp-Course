using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.BackgroundServices;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using EveryoneToTheHackathon.Infrastructure.Services;
using EveryoneToTheHackathon.Infrastructure.Strategies;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Application;

public class Startup(IConfiguration configuration)
{
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ConfigOptions>(configuration);
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<ConfigOptions>>();

        services.AddHostedService<HostedService>();

        services.AddSingleton<IDirectorService, DirectorService>();
        services.AddSingleton<IManagerService, ManagerService>();
        services.AddSingleton<IEmployeeService, EmployeeService>();
        services.AddSingleton<IHackathonService, HackathonService>();

        services.AddSingleton<ITeamBuildingStrategy, ManagerTeamBuildingStrategy>();

        services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        services.AddSingleton<IHackathonRepository, HackathonRepository>();

        services.AddDbContext<HackathonContext>(o =>
        {
            o.UseSqlServer(options.Value.Database!.DefaultConnection);
        });

        using var scope = services.BuildServiceProvider().CreateScope();
        {
            var context = scope.ServiceProvider.GetRequiredService<HackathonContext>();
            context.Database.Migrate();
        }
        
        var logRepository = (Hierarchy) LogManager.GetRepository();
        XmlConfigurator.Configure(logRepository, new FileInfo(options.Value.Logging!.ConfigFileName));
    }
    
    public void Configure(IApplicationBuilder app) {}
    
}