using Microsoft.Extensions.Configuration;
using EveryoneToTheHackathon.Options;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using EveryoneToTheHackathon.HR;
using EveryoneToTheHackathon.Strategy;
using log4net.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EveryoneToTheHackathon;

class Program
{
    static void Main(string[] args)
    {
        BindOptions();
        SetupLogger();
        
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<HackathonHost>();
                services.AddTransient<Hackathon>();
                services.AddTransient<ITeamBuildingStrategy, ManagerTeamBuildingStrategy>();
                services.AddSingleton<HRManager>();
                services.AddSingleton<HRDirector>();
            })
            .Build();
        host.Run();
    }

    static void BindOptions()
    {
        ConfigOptions configOptions = new();
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        config.GetSection("ParticipantList").Bind(configOptions);
        config.GetSection("Hackathon").Bind(configOptions);
        config.GetSection("Logging:LogConfig").Bind(configOptions);
    }

    static void SetupLogger()
    {
        var logRepository = (Hierarchy) LogManager.GetRepository();
        XmlConfigurator.Configure(logRepository, new FileInfo(ConfigOptions.LogFileName));
    }
}