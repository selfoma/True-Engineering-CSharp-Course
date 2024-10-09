using EveryoneToTheHackathon.HR;
using EveryoneToTheHackathon.Options;
using EveryoneToTheHackathon.Strategy;
using log4net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace EveryoneToTheHackathon;

public class HackathonHost : BackgroundService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(HackathonHost));
    
    private readonly IServiceProvider _serviceProvider;
    private readonly HRDirector _hrDirector;
    
    public HackathonHost(HRDirector hrDirector, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _hrDirector = hrDirector;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            for (int i = 0; i < ConfigOptions.timesToHold; ++i)
            {
                var hackathon = _serviceProvider.GetService<Hackathon>();
                _hrDirector.HoldHackathon(hackathon);
            }
            _hrDirector.ShowAverageHarmonic();
        }
        catch (NullReferenceException e)
        {
            Logger.Fatal("Null reference service");
            Logger.Fatal(e);
            Environment.Exit(-1);
        }

        await Task.CompletedTask;
    }

}