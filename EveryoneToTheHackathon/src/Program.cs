using Microsoft.Extensions.Configuration;
using EveryoneToTheHackathon.Options;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using EveryoneToTheHackathon.HR;

namespace EveryoneToTheHackathon;

class Program
{
    static void Main(string[] args)
    {
        BindOptions();
        SetupLogger();
        
        HRDirector hrDirector = new();
        hrDirector.HoldHackathon(1000);
        hrDirector.ShowAverageHarmonic();
    }

    static void BindOptions()
    {
        ConfigOptions configOptions = new();
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        config.GetSection("ParticipantList").Bind(configOptions);
        config.GetSection("Logging:LogConfig").Bind(configOptions);
    }

    static void SetupLogger()
    {
        var logRepository = (Hierarchy) LogManager.GetRepository();
        XmlConfigurator.Configure(logRepository, new FileInfo(ConfigOptions.LogFileName));
    }
}